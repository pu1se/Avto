using System;
using System.Collections.Generic;
using System.Text;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.RefreshAllProvidersTodayRates
{
    public class RefreshTodayRatesForProviderCommand : Command
    {
        public ExchangeProviderType? RefreshForProvider { get;set; }
    }
}
