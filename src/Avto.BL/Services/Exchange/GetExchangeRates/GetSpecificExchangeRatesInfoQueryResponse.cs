using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Exchange.GetExchangeRates
{
    public class GetSpecificExchangeRatesInfoQueryResponse
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime ExchangeDate { get; set; }
        public decimal OpenDayRate { get; set; }
        public decimal MinDayRate { get; set; }
        public decimal MaxDayRate { get; set; }
    }
}
