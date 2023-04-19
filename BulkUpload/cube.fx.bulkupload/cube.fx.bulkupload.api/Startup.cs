using cube.fx.bulkupload.api.Interfaces.Services;
using cube.fx.bulkupload.api.models;
using cube.fx.common.contract.interfaces;
using cube.fx.common.contract.model;
using cube.fx.common.dbclient;
using cube.fx.bulkupload.api.Models;
using cube.fx.common.helper;
using cube.fx.bulkupload.api.Services;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using cube.fx.bulkupload.api;
using cube.fx.bulkupload.api.Controllers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;

namespace cube.api.des.handler.common
{
    public class Startup
    {       
        public Startup(IConfiguration configuration)
        {           
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            Settings settings = new();
            Configuration.GetSection("Settings").Bind(settings);
            List<Context> contexts = new();            
            contexts = JsonConvert.DeserializeObject<List<Context>>(JsonConvert.SerializeObject(Configuration.GetSection("DBContext").GetChildren()));            
            services.AddTransient<ICRUD, CRUD>();
            services.AddTransient<BulkOrderInput>();
            services.AddTransient<DummyInput>();
            services.AddTransient<IService,DummyService>();
            services.AddTransient<IService, BulkProcessingService>();
            services.AddSingleton(settings);
            services.AddSingleton(contexts);

            Config.Settings = settings;
            Config.Contexts = contexts; 
            services.AddAuthentication();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "cube.fx.bulkupload.api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Common v1"));
            }

           
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();       
            app.UseAuthorization();
            
            // global error handler            
            app.UseMiddleware<ExceptionHandler>();
            app.UseMiddleware<AuthHandler>();
            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllers();
            });
        }
    }
}
