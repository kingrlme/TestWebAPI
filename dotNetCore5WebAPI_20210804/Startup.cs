using dotNetCore5WebAPI_0323.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace dotNetCore5WebAPI_0323
{
    public class Startup
    {
        private static readonly string[] DefaultPages = { "index.html", "index.htm", "default.html", "default.htm", "default.aspx" };
        private static string ROOT;

        public Startup(IConfiguration configuration)
        {   
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }


        // 1. �w��Swagger
        // 2.�bStartup.cs���U�P�ϥ�Swagger
        // 2.1.ConfigureServices ���A�ȵ��U
        // �ݨϥ�SwaggerGen ���USwagger ���\��A�å[�W�惡Api�M�ת��y�z�C
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // AddControllers �X�R��k�|���U MVC ����һݪ��A��
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Emma's First .net core Web API",
                    Description = "Use Swagger UI and RESTful API"
                });

                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Ū appsettings.json �N�i�H����Ū�� appsettings.json �����e
            // TEST: 
            // private static readonly string MSSQL_STR = Utils.Configuration.Databases.MSSQL_STR;
            // string MyKey = Utils.Configuration.MyKey;
            Utils.Configuration = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(Path.Combine(env.ContentRootPath, "appsettings.json")));

            ROOT = env.ContentRootPath;

            if (env.IsDevelopment())
            {
                // 2.2. Configure �[�W Swagger��Middleware
                //      �bPipeline �[�W Swagger��Middleware�A
                //      ��bapp.UseRouting���e�A���w���� json endpoint�P�W�١C
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                            $"/swagger/v1/swagger.json",
                            "Emma V1");
                    //options.RoutePrefix = string.Empty;
                });
                
                /*
                 3. �ק�launchSettings.json
                    �qVS����{�����ѼƷ|�̷�launchSettings.json�A
                    �]�tURL�P��L�����ܼơC
                    �ӭ���w�]�bIIS Express��launchUrl�Oweatherforecast�A
                    �N���令swagger�A�o��VS�ҥ�IIS���]�w�|��swagger�G
                 */



            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            


        }
    }
}
