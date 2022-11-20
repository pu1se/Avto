using System.Collections.Generic;
using Avto.BL.Services.Exchange.GetExchangeRates;

namespace Avto.UI.Front.ViewModels
{
    public class CurrencyChartPageModel : BasePageModel
    {
        public IEnumerable<string> CurrencyListSource { get; set; }
        public IEnumerable<GetSpecificExchangeRatesInfoQueryResponse> ExchangeListSource { get; set; }
        public GetSpecificExchangeRatesInfoQuery Filter { get; set; } = new GetSpecificExchangeRatesInfoQuery();
        public string Version { get; set; }
        public bool ShowTrend { get; set; }
        public bool ShowMean { get; set; }
    }
}
