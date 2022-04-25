using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.BL.Services.Currency.ResponseModels;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;
using Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs;
using Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders;
using Avto.DAL.Enums;

namespace Avto.UI.Front.ViewModels
{
    public class CurrencyConfigAddEditPageModel : BasePageModel
    {
        public bool IsAddingNewConfig { get; set; }
        public List<AvailableCurrencyResponse> AvailableCurrencyList { get; set; }
        public AddEditConfigModel Config { get; set; }
        public List<ExchangeProvidersResponse> ProviderList { get; set; }
    }
}
