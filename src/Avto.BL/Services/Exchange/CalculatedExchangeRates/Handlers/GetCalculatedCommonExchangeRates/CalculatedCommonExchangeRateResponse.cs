using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedCommonExchangeRates
{
    public class CalculatedCommonExchangeRateResponse
    {
        public string FromCurrencyCode { get; set; }
        public string ToCurrencyCode { get; set; }
        public DateTime ExchangeDate { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}
