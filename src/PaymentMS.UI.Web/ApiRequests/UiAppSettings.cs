using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PaymentMS.UI.Web.ApiRequests
{
    public class UiAppSettings
    {
        public UiAppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public string PaymentApiUrl => Configuration["PaymentApiUrl"];
        public string PaymentApiAuthClientId => Configuration["PaymentApiAuthenticationClientId"];
        public string PaymentApiAuthClientSecret => Configuration["PaymentApiAuthenticationClientSecret"];
    }
}
