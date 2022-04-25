using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.ExchangePrediction.Handlers.GetCurrencyExchangeInfo;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;

namespace PaymentMS.UI.Front.ApiRequests
{
    public class ExchangeReportApiClient : BaseApiClient
    {
        public ExchangeReportApiClient(UiAppSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<AvailableCurrencyResponse>>> GetAvailableCurrencies()
        {
            return Api.GetAsync<List<AvailableCurrencyResponse>>("currencies");
        }

        public Task<ApiCallDataResult<IEnumerable<CurrencyExchangeInfoItemResponse>>> GetExchangeReport(GetExchangeReportItemQuery query)
        {
            return Api.PostAsync<IEnumerable<CurrencyExchangeInfoItemResponse>>($"exchange/report", query);
        }
    }
}
