using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedCommonExchangeRates
{
    public class GetCalculatedCommonExchangeRatesQuery : Query
    {
        [NotEmptyValueRequired]
        public DateTime ExchangeDate { get; set; }
    }
}
