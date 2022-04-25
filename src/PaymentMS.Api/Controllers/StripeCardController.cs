using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.BL.Services;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.Payments;
using PaymentMS.BL.Services.Stripe.ResponseModels;
using PaymentMS.DAL.CloudServices;

namespace PaymentMS.Api.Controllers
{
    [Route("stripe-card")]
    public class StripeCardController : BaseApiController
    {
        private StripeService StripeService { get; }
        
        public StripeCardController(StripeService paymentStripeService)
        {
            StripeService = paymentStripeService;
        }

        [HttpPost]
        [Route("receivers")]
        public async Task<IActionResult> AddReceivingWay([FromBody]AddReceivingWayCommand model)
        {
            return await HttpResponse(() => StripeService.AddReceivingWay(model));
        }
        
        [HttpGet]
        [Route("receivers")]
        public async Task<IActionResult> GetReceivingWays()
        {
            return await HttpResponse(() => StripeService.GetAllReceivingWays());
        }

        [HttpPut]
        [Route("receivers/{receiverOrganizationId:guid}/senders")]
        public async Task<IActionResult> AddOrEditSendingWay(
            Guid receiverOrganizationId,
            [FromBody]AddEditSendingWayCommand model)
        {
            if (model != null)
            {
                model.ReceiverOrganizationId = receiverOrganizationId;
            }
            
            return await HttpResponse(() => StripeService.AddEditSendingWay(model));
        }

        [HttpGet]
        [Route("receivers/{receiverOrganizationId:guid}/senders/{senderOrganizationId:guid}/default-card")]
        public async Task<IActionResult> GetDefaultSendingWayByOrganizations(Guid receiverOrganizationId, Guid senderOrganizationId)
        {
            return await HttpResponse(() => StripeService.GetDefaultSendingWay(new GetDefaultSendingWayQuery
            {
                ReceiverOrganizationId = receiverOrganizationId,
                SenderOrganizationId = senderOrganizationId
            }));
        }

        [HttpPost]
        [Route("receivers/{receiverOrganizationId:guid}/senders/{senderOrganizationId:guid}/payments")]
        public async Task<IActionResult> SendPayment(
            Guid receiverOrganizationId,
            Guid senderOrganizationId,
            [FromBody]SendPaymentCommand model)
        {
            if (model != null)
            {
                model.ReceiverOrganizationId = receiverOrganizationId;
                model.SenderOrganizationId = senderOrganizationId;
            }

            return await HttpResponse(async () =>
            {
                var payment = await StripeService.SendPayment(model);
                if (payment.IsSuccess)
                {
                    payment.Data.PaymentRefundUrl = GetPaymentRefundUrl(payment.Data);
                }

                return payment;
            });
        }

        private string GetPaymentRefundUrl(SendPaymentCommandResponse model)
        {
            if (model == null)
            {
                return string.Empty;
            }
            return $"{GetBaseHostUrl()}/stripe-card/receivers/{model.ReceiverOrganizationId}/senders/{model.SenderOrganizationId}/payments/{model.PaymentId}/refund";
        }

        [HttpPost]
        [Route("receivers/{receiverOrganizationId:guid}/senders/{senderOrganizationId:guid}/payments/{paymentId:guid}/refund")]
        public async Task<IActionResult> RefundPayment(
            Guid receiverOrganizationId,
            Guid senderOrganizationId,
            Guid paymentId)
        {
            return await HttpResponse(() => StripeService.PaymentRefund(new PaymentRefundCommand
            {
                ReceiverOrganizationId = receiverOrganizationId,
                SenderOrganizationId = senderOrganizationId,
                PaymentId = paymentId
            }));
        }

        [HttpGet]
        [Route("receivers/{receiverOrganizationId:guid}/senders/{senderOrganizationId:guid}/payments")]
        public async Task<IActionResult> GetPaymentList(Guid receiverOrganizationId, Guid senderOrganizationId)
        {
            var query = new GetPaymentsQuery
            {
                SenderOrganizationId = senderOrganizationId,
                ReceiverOrganizationId = receiverOrganizationId
            };
            return await HttpResponse(() => StripeService.GetPayments(query));
        }

        [HttpGet]
        [Route("senders/{senderOrganizationId:guid}/payments")]
        public async Task<IActionResult> GetPaymentList(Guid senderOrganizationId)
        {
            var query = new GetPaymentsQuery
            {
                SenderOrganizationId = senderOrganizationId,
            };
            return await HttpResponse(() => StripeService.GetPayments(query));
        }

    }
}
