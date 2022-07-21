using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Avto.UI.Front.ApiRequests
{
    public class UiAppSettings
    {
        public UiAppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
            PaymentApiUrl = Configuration["PaymentApiUrl"];
            PaymentApiAuthClientId = Configuration["PaymentApiAuthenticationClientId"];
            PaymentApiAuthClientSecret = Configuration["PaymentApiAuthenticationClientSecret"];
        }


        private IConfiguration Configuration { get; }

        public string PaymentApiUrl { get; }
        public string PaymentApiAuthClientId { get; }
        public string PaymentApiAuthClientSecret { get; }
    }
}
