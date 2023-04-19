using cube.fx.common.contract.model;
using cube.fx.common.dbclient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Net.Http;
using System;
using cube.fx.common.contract.interfaces;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cube.fx.common.helper
{
    public class AuthHandler
    {
        private readonly RequestDelegate _next;
        private readonly ICRUD _instance;
        private bool isValid = false;
        public AuthHandler(RequestDelegate next, ICRUD iCRUD)
        {
            _instance = iCRUD;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.Path.HasValue && !context.Request.Path.Value.ToLower().Contains("/swagger/"))
                {
                    APIResponse aPIResponse = new APIResponse();
                    OnAuthorization(context);
                    if (isValid)
                        await _next(context);
                    else
                        await Task.Run(() =>
                        {
                            aPIResponse.Status = "Failed";
                            aPIResponse.Error = "Authorization Failed";
                            string str = JsonConvert.SerializeObject(aPIResponse);
                            context.Response.ContentType = "application/json";
                            context.Response.WriteAsync(str);


                        });
                }
                else
                    await _next(context);
            }
            catch
            {
                throw;
            }
        }
        public void OnAuthorization(HttpContext actionContext)
        {
            string[] strinList = Config.Settings.CommonSettings.SkipAuth.Split(',');
            
            if (actionContext != null)
            {
                Config.Settings.CommonSettings.AccessToken = actionContext.Request.Headers["Authorization"];
                //if (Config.Settings.CommonSettings.AccessToken == null)
                //    return;
                if (!StringCheck(strinList, actionContext.Request.Path.Value))
                {
                    ApiCall objApiCall = new ApiCall();
                    ApiCallEntity apiCallEntity = new ApiCallEntity { BaseAPIEndPoint = Config.Settings.CommonSettings.AuthApiURL, ApiURL = "Token/GetTokenClaim", HttpMethod = "Get", AccessToken = Config.Settings.CommonSettings.AccessToken };
                    APIResponse response = objApiCall.ExternalAPICall(apiCallEntity);

                    TokenClaims tc = JsonConvert.DeserializeObject<TokenClaims>((string)response.Response);
                    
                    if (tc.UserName!=null)
                    {
                        actionContext.Items["tokens"] = tc;
                        Config.Settings.CommonSettings.UserEmailId = tc.Email;
                        Config.Settings.CommonSettings.UserName = tc.UserName;
                        Config.Settings.CommonSettings.TenantCode = tc.TenantCode;
                        Config.Settings.CommonSettings.SAP_FK = Guid.Parse(tc.SAP_FK);                        
                        Config.Settings.CommonSettings.TokenClaim = tc;                        
                        isValid = true;
                    }
                    else
                        HandleUnauthorizedRequest(actionContext);
                }
                else
                {
                    isValid = true;
                }
            }
            //if (!BasicDetail.StringCheck(strinList, ""))
            //{
            //    TokenClaims tc = BasicDetail.GetBasicDetails();
            //    if (tc != null)
            //        actionContext.HttpContext.Items["tokens"] = tc;
            //    else
            //        HandleUnauthorizedRequest(actionContext);

            //bool IsApiAuthorized = true; // Convert.ToBoolean(ConfigurationManager.AppSettings["IsApiAuthorized"]);

            //if (IsApiAuthorized && tc != null && tc.TokenType == "PRIVATEKEY")
            //    IsAuthorized = CheckValidApiAccess(actionContext, IsApiAuthorized, tc);

            //if (!IsAuthorized)
            //    HandleUnauthorizedRequest(actionContext);

            // bool IsClearOldSession = Convert.ToBoolean(ConfigurationManager.AppSettings["ClearOldSession"]);


            // bool IsTokenExpired = CheckTokenExpired(actionContext, tc);
            //if (!IsTokenExpired && tc != null && tc.UserType != "ServiceAccount")
            //    HandleUnauthorizedRequest(actionContext);
            //else
            //{
            //    if (tc != null)
            //    {
            //        external call
            //        UpdateSecUserSession(actionContext, tc);
            //    }
            //}

            //var organizationCode = actionContext.HttpContext.Authentication;
            //var identity = ((ClaimsPrincipal)organizationCode.User).Identity as ClaimsIdentity;
            //var claimsValue = organizationCode.User.Identities.First().Claims.FirstOrDefault();
            //}
        }
        protected void HandleUnauthorizedRequest(HttpContext actionContext)
        {
            //HttpActionContext httpActionContext = new HttpActionContext();
            //var response = httpActionContext.Request.CreateResponse<APIResponse>
            //                     (new APIResponse()
            //                     {
            //                         Id = 1,
            //                         Count = 1,
            //                         Status = "Failed",
            //                         Messages = new List<Message>() { new Message() { Type = "Error", MessageDesc = "Authorization has been denied for this request" } }
            //                     });
            //response.StatusCode = HttpStatusCode.Unauthorized;
            AuthenticationProperties authenticationProperties = new AuthenticationProperties();
            authenticationProperties.IssuedUtc = DateTime.UtcNow;
            actionContext.ForbidAsync("Auth");

        }

        public static bool StringCheck(string[] strList, string str)
        {
            bool check = false;
            //int temp = Array.IndexOf(strList, str);
            int temp = Array.FindIndex(strList, t => t.IndexOf(str, StringComparison.InvariantCultureIgnoreCase) >= 0);
            if (temp > -1)
                check = true;
            else
                check = false;

            return check;
        }
    }
}

