using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.BL.Services.Exchange.ExchangePrediction.Handlers.GetCurrencyExchangeInfo
{
    public class GetExchangeReportItemQuery
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public int PeriodInDays { get; set; }
    }
}
