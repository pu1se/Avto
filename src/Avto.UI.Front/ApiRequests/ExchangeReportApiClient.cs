using System.Collections.Generic;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.ExchangePrediction.Handlers.GetCurrencyExchangeInfo;
using Avto.BL.Services.Exchange.ExchangeProviders.Handlers.Queries.GetAvailableCurrencies;

namespace Avto.UI.Front.ApiRequests
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
