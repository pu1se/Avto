using System.Collections.Generic;
using PaymentMS.BL.Services.Exchange.ExchangePrediction.Handlers.GetCurrencyExchangeInfo;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;

namespace PaymentMS.UI.Front.ViewModels
{
    public class CurrencyChartPageModel : BasePageModel
    {
        public IEnumerable<string> CurrencyListSource { get; set; }
        public IEnumerable<CurrencyExchangeInfoItemResponse> ExchangeListSource { get; set; }
        public GetExchangeReportItemQuery Filter { get; set; } = new GetExchangeReportItemQuery();
        public Dictionary<int, string> PeriodInDaysSource { get; set; }
    }
}
