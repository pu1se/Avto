using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.ExchangeRates.Handlers.GetRatesByOrganization;
using Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeConfigs;
using Avto.UI.Front.ViewModels;

namespace Avto.UI.Front.ApiRequests
{
    public class ExchangeOrganizationConfigApiClient : BaseApiClient
    {
        public ExchangeOrganizationConfigApiClient(UiAppSettings settings) : base(settings)
        {
        }

        public Task<ApiCallDataResult<List<ConfigViewModel>>> GetConfigsAsync(Guid organizationId)
        {
            return Api.GetAsync<List<ConfigViewModel>>(
                $"exchange/organizations/{organizationId}/configs"
            );
        }

        public Task<ApiCallResult> AddEditConfigAsync(AddEditConfigModel command)
        {
            return Api.PutAsync(
                $"exchange/organizations/{command.OrganizationId}/configs",
                command
            );
        }

        public Task<ApiCallResult> DeleteConfigAsync(AddEditConfigModel command)
        {
            return Api.DeleteAsync(
                $"exchange/organizations/{command.OrganizationId}/configs",
                command
            );
        }

        public Task<ApiCallDataResult<List<RateResponse>>> GetRatesAsync(Guid organizationId)
        {
            return Api.GetAsync<List<RateResponse>>(
                $"exchange/organizations/{organizationId}/rates"
            );
        }

        public void FillConfigWithRate(ConfigViewModel config, List<RateResponse> rateList)
        {
            var rate = rateList.FirstOrDefault(
                x => 
                    x.FromCurrency == config.FromCurrency &&
                    x.ToCurrency == config.ToCurrency
            );

            if (rate == null)
            {
                return;
            }

            config.ExchangeRate = rate.ExchangeRate;
        }
    }
}
