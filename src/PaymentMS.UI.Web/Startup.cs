using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaymentMS.BL;
using PaymentMS.UI.Web.ApiRequests;
using Stripe;

namespace PaymentMS.UI.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddRazorPages();

            services.AddTransient<UiAppSettings>();
            services.AddTransient<StripeCardApiClient>();
            services.AddTransient<BalanceApiClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                bool isAjaxCall = context.Request.Headers["x-requested-with"] == "XMLHttpRequest";
                if (isAjaxCall)
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync($"<script>alert(`{exception.Message.Replace('`', '\'')}`)</script>");
                }
                else
                {
                    await context.Response.WriteAsync($"<!DOCTYPE html><html><body><h4 style='color:darkred;'>{exception.Message}</h4></body></html>");
                }
                
            }));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Receiver}/{action=AddReceiver}/{id?}");
            });
        }
    }
}
