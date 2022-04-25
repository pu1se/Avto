using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates
{
    public class TodayProviderExchangeRateResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ExchangeProviderType ExchangeProvider { get; set; }
        public DateTime ExchangeDate { get; set; }
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
