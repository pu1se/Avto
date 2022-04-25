using System;
using System.Collections.Generic;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.ExchangeProviders.Handlers.RefreshAllProvidersTodayRates
{
    public class RefreshTodayRatesForProviderCommand : Command
    {
        public ExchangeProviderType? RefreshForProvider { get;set; }
    }
}
