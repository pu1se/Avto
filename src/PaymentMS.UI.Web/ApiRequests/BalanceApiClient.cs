using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.UI.Web.ViewModels.Balance;

namespace PaymentMS.UI.Web.ApiRequests
{
    public class BalanceApiClient
    {
        private ApiRequestExecuter Api { get; }
        private UiAppSettings Settings { get; }

        public BalanceApiClient(UiAppSettings settings)
        {
            Settings = settings;
            Api = new ApiRequestExecuter(Settings.PaymentApiUrl, settings.PaymentApiAuthClientId, settings.PaymentApiAuthClientSecret);
        }

        public Task<ApiCallDataResult<List<BalanceProviderOrganizationModel>>> GetPossibleProviderOrganizationsAsync()
        {
            return Api.GetAsync<List<BalanceProviderOrganizationModel>>("balance/possible-provider-organizations");
        }

        public Task<ApiCallResult> AddProvider(object model)
        {
            return Api.PutAsync("balance/providers", model);
        }               

        public Task<ApiCallDataResult<BalanceProviderModel>> GetProvider(Guid organizationId)
        {
            return Api.GetAsync<BalanceProviderModel>("balance/providers/"+organizationId);
        }

        public Task<ApiCallDataResult<List<BalanceProviderModel>>> GetProviders()
        {
            return Api.GetAsync<List<BalanceProviderModel>>("balance/providers/");
        }

        public Task<ApiCallDataResult<BalanceClientModel>> GetClient(Guid providerOrganizationId, Guid clientOrganizationId)
        {
            return Api.GetAsync<BalanceClientModel>($"balance/providers/{providerOrganizationId}/clients/{clientOrganizationId}");
        }

        public Task<ApiCallDataResult<List<BalancePaymentModel>>> GetPaymentHistory(Guid providerOrganizationId, Guid clientOrganizationId)
        {
            return Api.GetAsync<List<BalancePaymentModel>>($"balance/providers/{providerOrganizationId}/clients/{clientOrganizationId}/payments");
        }

        public Task<ApiCallDataResult<List<BalanceClientModel>>> GetClientList(Guid providerOrganizationId)
        {
            return Api.GetAsync<List<BalanceClientModel>>($"balance/providers/{providerOrganizationId}/clients");
        }

        public Task<ApiCallResult> AddClient(AddBalanceClientCommand model)
        {
            return Api.PostAsync($"balance/providers/{model.ProviderOrganizationId}/clients", model);
        }

        public Task<ApiCallResult> IncreaseByWireTransfer(IncreaseByWireTransferViewModel model)
        {
            return Api.PostAsync($"balance/providers/{model.SelectedProviderId}/" +
                                $"clients/{model.SelectedClientId}/" +
                                $"increase/wire-transfer", model);
        }

        public Task<ApiCallResult> Pay(SendBalancePaymentViewModel model)
        {
            return Api.PostAsync($"balance/providers/{model.SelectedProviderId}/" +
                                 $"clients/{model.SelectedClientId}/" +
                                 $"payments", model);
        }

        public Task<ApiCallResult> IncreaseByCard(IncreaseBalanceByStripeCardCommand command)
        {
            return Api.PostAsync($"balance/providers/{command.ProviderOrganizationId}/" +
                                 $"clients/{command.ClientOrganizationId}/" +
                                 $"increase/stripe-card", command);
        }
    }
}
