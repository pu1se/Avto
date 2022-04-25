using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Exchange.CalculatedExchangeRates.Handlers.GetCalculatedConfiguredByOrganizationExchangeRates
{
    public class CalculatedConfiguredByOrganizationExchangeRateResponse
    {
        public string FromCurrencyCode { get; set; }
        public string ToCurrencyCode { get; set; }
        public DateTime ExchangeDate { get; set; }
        public decimal ExchangeRate { get; set; }
        public Guid ProvidedByOrganizationId { get; set; }
    }
}
