using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.AllReceivingWays;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.Payments;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.BL.Services.Stripe.ResponseModels;
using PaymentMS.UI.Web.ApiRequests;
using PaymentMS.UI.Web.ViewModels;

namespace PaymentMS.UI.Web
{
    public class StripeCardApiClient
    {
        private ApiRequestExecuter Api { get; }
        private UiAppSettings Settings { get; }

        public StripeCardApiClient(UiAppSettings settings)
        {
            Settings = settings;
            Api = new ApiRequestExecuter(Settings.PaymentApiUrl, settings.PaymentApiAuthClientId, settings.PaymentApiAuthClientSecret);
        }

        public Task<ApiCallResult> AddSendingWayAsync(AddEditSendingWayCommand model)
        {
            return Api.PutAsync($"stripe-card/receivers/{model.ReceiverOrganizationId}/senders", model);
        }

        public Task<ApiCallResult> AddReceivingWayAsync(AddStripeReceiverViewModel model)
        {
            return Api.PostAsync("stripe-card/receivers", model);
        }
        
        
        public Task<ApiCallDataResult<List<ReceivingWayQueryResponse>>> GetAllReceivingWaysAsync()
        {
            return Api.GetAsync<List<ReceivingWayQueryResponse>>("stripe-card/receivers");
        }

        public Task<ApiCallDataResult<SendPaymentCommandResponse>> SendPaymentAsync(SendPaymentCommand model)
        {
            return Api.PostAsync<SendPaymentCommandResponse>($"stripe-card/receivers/{model.ReceiverOrganizationId}/senders/{model.SenderOrganizationId}/payments", model);
        }

        public Task<ApiCallResult> RefundPaymentAsync(string refundPaymentUrl)
        {
            return Api.PostAsync(refundPaymentUrl);
        }

        public Task<ApiCallDataResult<List<PaymentQueryResponse>>> GetPaymentsAsync(Guid receiverOrganizationId, Guid senderOrganizationId)
        {
            return Api.GetAsync<List<PaymentQueryResponse>>($"stripe-card/receivers/{receiverOrganizationId}/senders/{senderOrganizationId}/payments");
        }

        public async Task<ApiCallDataResult<DefaultSendingWayQueryResponse>> GetDefaultSendingWayByOrgAsync(Guid senderOrganizationId, Guid receiverOrganizationId)
        {
            var result = await Api.GetAsync<DefaultSendingWayQueryResponse>($"stripe-card/receivers/{receiverOrganizationId}/senders/{senderOrganizationId}/default-card");
            return result;
        }

        public Task<ApiCallDataResult<List<PaymentQueryResponse>>> GetPaymentsAsync(Guid sernderOrganizationId)
        {
            return Api.GetAsync<List<PaymentQueryResponse>>($"stripe-card/senders/{sernderOrganizationId}/payments");
        }


    }
}
