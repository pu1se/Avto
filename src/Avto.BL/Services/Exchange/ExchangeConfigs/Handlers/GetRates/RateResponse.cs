using System;
using System.Collections.Generic;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization
{
    public class RateResponse
    {
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ExchangeDate { get; set; }
        public Guid ProvidedByOrganizationId { get; set; }
        public decimal ExchangeRateWithSurcharge { get; set; }
    }
}
