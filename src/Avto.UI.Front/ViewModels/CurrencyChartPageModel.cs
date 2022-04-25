using System.Collections.Generic;
using Avto.BL.Services.Exchange.ExchangePrediction.Handlers.GetCurrencyExchangeInfo;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;

namespace Avto.UI.Front.ViewModels
{
    public class CurrencyChartPageModel : BasePageModel
    {
        public IEnumerable<string> CurrencyListSource { get; set; }
        public IEnumerable<CurrencyExchangeInfoItemResponse> ExchangeListSource { get; set; }
        public GetExchangeReportItemQuery Filter { get; set; } = new GetExchangeReportItemQuery();
        public Dictionary<int, string> PeriodInDaysSource { get; set; }
    }
}
