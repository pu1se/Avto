using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Avto
{
    public class AppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            DatabaseConnection = configuration["DefaultConnection"];
            AuthClientId = configuration["Authentication:ClientId"];
            AuthClientSecret = configuration["Authentication:ClientSecret"];
            Environment = configuration["Environment"];
            CurrencyLayerApiKey = configuration["CurrencyLayer:ApiKey"];
        }

        public string DatabaseConnection { get; }
        public string AuthClientId { get; }
        public string AuthClientSecret { get; }
        public string Environment { get; }
        public string CurrencyLayerApiKey { get; }
    }
}
