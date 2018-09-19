using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroService.Middleware;
using Swashbuckle.Swagger.Model;

namespace derNotrneldesDiensts
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.


            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

        

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            // Enabling CORS....
            // app.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader().WithExposedHeaders("authorization", "Authorization", "accept", "content-type");


            });


      
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    // Disable caching for all static files.
                    context.Context.Response.Headers["Cache-Control"] =
                    Configuration["StaticFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] =
                    Configuration["StaticFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"] =
                    Configuration["StaticFiles:Headers:Expires"];
                }
            });


            //// Registering Swagger middleware

            //app.UseSwagger();
            //app.UseSwaggerUi();

            //// Registering JWT middleware
            app.UseJWTMiddleware();

            app.UseMvc();



            // Swagger Address
            //http://localhost:22511/swagger/ui/index.html
        }
    }
}
