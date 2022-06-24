using System.Collections.Generic;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.GetAvailableCurrencies;
using Avto.BL.Services.Exchange.GetExchangeRates;

namespace Avto.UI.Front.ApiRequests
{
    public class ExchangeApiClient : BaseApiClient
    {
        public ExchangeApiClient(UiAppSettings settings) : base(settings)
        {
        }
        
        public Task<ApiCallDataResult<List<AvailableCurrencyResponse>>> GetAvailableCurrencies()
        {
            return Api.GetAsync<List<AvailableCurrencyResponse>>("exchange/currencies");
        }

        public Task<ApiCallDataResult<IEnumerable<GetSpecificExchangeRatesInfoQueryResponse>>> GetExchangeReport(GetSpecificExchangeRatesInfoQuery query)
        {
            return Api.PostAsync<IEnumerable<GetSpecificExchangeRatesInfoQueryResponse>>($"exchange", query);
        }
    }
}
