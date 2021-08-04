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


        // 1. 安裝Swagger
        // 2.在Startup.cs註冊與使用Swagger
        // 2.1.ConfigureServices 的服務註冊
        // 需使用SwaggerGen 註冊Swagger 的功能，並加上對此Api專案的描述。
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // AddControllers 擴充方法會註冊 MVC 控制器所需的服務
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
            // 讀 appsettings.json 就可以直接讀取 appsettings.json 的內容
            // TEST: 
            // private static readonly string MSSQL_STR = Utils.Configuration.Databases.MSSQL_STR;
            // string MyKey = Utils.Configuration.MyKey;
            Utils.Configuration = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(Path.Combine(env.ContentRootPath, "appsettings.json")));

            ROOT = env.ContentRootPath;

            if (env.IsDevelopment())
            {
                // 2.2. Configure 加上 Swagger的Middleware
                //      在Pipeline 加上 Swagger的Middleware，
                //      放在app.UseRouting之前，指定它的 json endpoint與名稱。
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
                 3. 修改launchSettings.json
                    從VS執行程式的參數會依照launchSettings.json，
                    包含URL與其他環境變數。
                    而原先預設在IIS Express的launchUrl是weatherforecast，
                    將它改成swagger，這樣VS啟用IIS的設定會用swagger：
                 */



            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            


        }
    }
}
