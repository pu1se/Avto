using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Currency.ResponseModels;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;
using PaymentMS.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs;
using PaymentMS.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders;
using PaymentMS.DAL.Enums;

namespace PaymentMS.UI.Front.ViewModels
{
    public class CurrencyConfigAddEditPageModel : BasePageModel
    {
        public bool IsAddingNewConfig { get; set; }
        public List<AvailableCurrencyResponse> AvailableCurrencyList { get; set; }
        public AddEditConfigModel Config { get; set; }
        public List<ExchangeProvidersResponse> ProviderList { get; set; }
    }
}
