using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lombok.NET.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#if DEBUG
using System.Diagnostics;
#endif

namespace Lombok.NET.MethodGenerators;

/// <summary>
/// Generator which generates a ToString implementation for a type.
/// </summary>
[Generator]
public sealed class ToStringGenerator : IIncrementalGenerator
{
	/// <summary>
	/// Initializes the generator logic.
	/// </summary>
	/// <param name="context">The context of initializing the generator.</param>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
//#if DEBUG
//        SpinWait.SpinUntil(static () => Debugger.IsAttached);
//#endif
		var sources = context.SyntaxProvider.CreateSyntaxProvider(IsCandidate, Transform).Where(static s => s != null);
		context.AddSources(sources);
	}

	private static bool IsCandidate(SyntaxNode node, CancellationToken cancellationToken)
	{
		TypeDeclarationSyntax? typeDeclaration = node as ClassDeclarationSyntax;
		typeDeclaration ??= node as StructDeclarationSyntax;
		if (typeDeclaration is null)
		{
			return false;
		}

		return typeDeclaration.AttributeLists
			.SelectMany(static l => l.Attributes)
			.Any(static a => a.IsNamed("ToString"));
	}

	private static GeneratorResult Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
	{
		SymbolCache.ToStringAttributeSymbol ??= context.SemanticModel.Compilation.GetSymbolByType<ToStringAttribute>();
		
		var typeDeclaration = (TypeDeclarationSyntax)context.Node;
		if (!typeDeclaration.ContainsAttribute(context.SemanticModel, SymbolCache.ToStringAttributeSymbol)) 
		{
			return GeneratorResult.Empty;
		}
		
		if (!typeDeclaration.TryValidateType(out var @namespace, out var diagnostic))
		{
			return new GeneratorResult(diagnostic);
		}
		
		var toStringMethod = CreateToStringMethod(typeDeclaration);
		if (toStringMethod is null)
		{
			return GeneratorResult.Empty;
		}
		
		cancellationToken.ThrowIfCancellationRequested();

		var sourceText = CreateType(@namespace, typeDeclaration.CreateNewPartialType(), toStringMethod);

		return new GeneratorResult(typeDeclaration.Identifier.Text, sourceText);
	}

	private static MethodDeclarationSyntax? CreateToStringMethod(TypeDeclarationSyntax typeDeclaration)
	{
		var memberType = typeDeclaration.GetAttributeArgument<MemberType>("ToString") ?? MemberType.Field;
		var accessType = typeDeclaration.GetAttributeArgument<AccessTypes>("ToString") ?? AccessTypes.Private;

		string[] identifiers;
		switch (memberType)
		{
			case MemberType.Property:
				identifiers = typeDeclaration.Members
					.OfType<PropertyDeclarationSyntax>()
					.Where(accessType)
					.Select(static p => p.Identifier.Text)
					.ToArray();

				break;
			case MemberType.Field:
				identifiers = typeDeclaration.Members
					.OfType<FieldDeclarationSyntax>()
					.Where(accessType)
					.SelectMany(static f => f.Declaration.Variables.Select(static v => v.Identifier.Text))
					.ToArray();

				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(memberType));
		}

		if (identifiers.Length == 0)
		{
			return null;
		}

		var stringInterpolationContent = new List<InterpolatedStringContentSyntax>
		{
			CreateStringInterpolationContent(typeDeclaration.Identifier.Text + ": "),
			CreateStringInterpolationContent(identifiers[0] + "="),
			Interpolation(
				IdentifierName(identifiers[0])
			)
		};

		for (int i = 1; i < identifiers.Length; i++)
		{
			stringInterpolationContent.Add(CreateStringInterpolationContent("; " + identifiers[i] + "="));
			stringInterpolationContent.Add(Interpolation(IdentifierName(identifiers[i])));
		}

		return MethodDeclaration(
			PredefinedType(
				Token(SyntaxKind.StringKeyword)
			),
			"ToString"
		).WithModifiers(
			TokenList(
				Token(SyntaxKind.PublicKeyword),
				Token(SyntaxKind.OverrideKeyword)
			)
		).WithBody(
			Block(
				ReturnStatement(
					InterpolatedStringExpression(
						Token(SyntaxKind.InterpolatedStringStartToken)
					).WithContents(List(stringInterpolationContent))
				)
			)
		);
	}

	private static InterpolatedStringContentSyntax CreateStringInterpolationContent(string s)
	{
		return InterpolatedStringText(
			Token(
				TriviaList(),
				SyntaxKind.InterpolatedStringTextToken,
				s,
				s,
				TriviaList()
			)
		);
	}

	private static SourceText CreateType(string @namespace, TypeDeclarationSyntax typeDeclaration, MethodDeclarationSyntax toStringMethod)
	{
		return FileScopedNamespaceDeclaration(
				IdentifierName(@namespace)
			).WithMembers(
				SingletonList<MemberDeclarationSyntax>(
					typeDeclaration.WithMembers(
						new SyntaxList<MemberDeclarationSyntax>(toStringMethod)
					)
				)
			).NormalizeWhitespace()
			.GetText(Encoding.UTF8);
	}
}