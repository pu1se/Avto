using System;
using System.Reflection;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Avto.Api._Core;
using Avto.Api.JobsScheduler;
using Avto.BL;

namespace Avto.Api
{
    public class Startup
    {
        private AppSettings AppSettings { get; }

        public Startup(IWebHostEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange:true)                
                .AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();
            AppSettings = new AppSettings(configuration);
        }        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)
                .AddOAuthValidation()
                .AddOpenIdConnectServer(options =>
            {
                options.AllowInsecureHttp = false;
                options.ApplicationCanDisplayErrors = true;
                options.AccessTokenLifetime = TimeSpan.FromDays(1);
                options.TokenEndpointPath = "/token";
                options.Provider = new OAuthProvider(AppSettings);
            });


            services.AddMvcCore().AddApiExplorer();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(settings =>
            {
                var version = typeof(Startup)
                    .GetTypeInfo()
                    .Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;
                var build = GetCurrentBuildVersionString();
                settings.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Exchange Microservice Api",
                    Version = $"v{version}.{build}",
                });
            });

            DependencyManager.Configure(services, AppSettings);

            Scheduler.StartAllScheduledJobs(services);
        }

        // todo: save build version to project file.
        private static string _buildVersion;
        private static string GetCurrentBuildVersionString()
        {
            if (_buildVersion.IsNullOrEmpty())
            {
                _buildVersion = (DateTime.Today - new DateTime(2022, 6, 24)).Days.ToString();
            }
            return _buildVersion;
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStatusCodePages();

            app.UseMiddleware<LoggerMiddleware>();
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(settings =>
            {
                settings.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Microservice Api");
                settings.RoutePrefix = string.Empty;
            });
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
