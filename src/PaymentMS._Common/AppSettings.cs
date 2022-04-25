using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PaymentMS
{
    public class AppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            DatabaseConnection = configuration["DefaultConnection"];
            AuthClientId = configuration["Authentication:ClientId"];
            AuthClientSecret = configuration["Authentication:ClientSecret"];
            Environment = configuration["Environment"];
            AzureKeyVaultUrl = configuration["AzureKeyVault:Url"];
            Currency = new CurrencyConfiguration(configuration);
            CurrencyLayerApiKey = configuration["CurrencyLayer:ApiKey"];
        }

        public string DatabaseConnection { get; }
        public string AuthClientId { get; }
        public string AuthClientSecret { get; }
        public string Environment { get; }
        public string AzureKeyVaultUrl { get; }    
        public CurrencyConfiguration Currency { get; }
        public string CurrencyLayerApiKey { get; }
    }

    public class CurrencyConfiguration
    {
        public decimal ExchangeRiskSurchargeAsPercent { get; }
        public decimal ExchangeRiskSurchargeAsPercentForUSD { get; }
        public decimal ExchangeRiskSurchargeAsPercentForSEK { get; }
        public decimal ExchangeRiskSurchargeAsPercentForDKK { get; }
        public decimal ExchangeRiskSurchargeAsPercentForNOK { get; }


        public CurrencyConfiguration(IConfiguration configuration)
        {
            var exchangeSurcharge = ParseToDecimal(configuration["Currency:ExchangeRiskSurchargeAsPercent"]);
            var defaultSurcharge = exchangeSurcharge ?? 0;
            ExchangeRiskSurchargeAsPercent = defaultSurcharge;

            var usdSurcharge = ParseToDecimal(configuration["Currency:ExchangeRiskSurchargeAsPercentForUSD"]);
            ExchangeRiskSurchargeAsPercentForUSD = usdSurcharge ?? defaultSurcharge;

            var sekSurcharge = ParseToDecimal(configuration["Currency:ExchangeRiskSurchargeAsPercentForSEK"]);
            ExchangeRiskSurchargeAsPercentForSEK = sekSurcharge ?? defaultSurcharge;

            var dkkSurcharge = ParseToDecimal(configuration["Currency:ExchangeRiskSurchargeAsPercentForDKK"]);
            ExchangeRiskSurchargeAsPercentForDKK = dkkSurcharge ?? defaultSurcharge;

            var nokSurcharge = ParseToDecimal(configuration["Currency:ExchangeRiskSurchargeAsPercentForNOK"]);
            ExchangeRiskSurchargeAsPercentForNOK = nokSurcharge ?? defaultSurcharge;
        }

        private decimal? ParseToDecimal(string numberAsString)
        {
            if (!numberAsString.IsNullOrEmpty())
            {
                numberAsString = numberAsString.Replace(",", ".");
            }

            if (decimal.TryParse(numberAsString, out var number))
            {
                return number;
            }

            return null;
        }
    }
}
