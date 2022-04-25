using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies
{
    public class AvailableCurrencyResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
