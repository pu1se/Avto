using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Stripe.Api;
using Avto.BL.Services.Stripe.Handlers.PaymentRefund;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Stripe.Handlers
{
    public class PaymentRefundCommandHandler : CommandHandler<PaymentRefundCommand, CallDataResult<PaymentRefundCommandResponse>>
    {
        private SafeCallStripeApi StripeApi { get; }

        public PaymentRefundCommandHandler(
            Storage storage, 
            SafeCallStripeApi stripeApi,
            LogService logger) 
            : base(storage, logger)
        {
            StripeApi = stripeApi;
        }

        protected override async Task<CallDataResult<PaymentRefundCommandResponse>> HandleCommandAsync(PaymentRefundCommand command)
        {
            var receivingWay = await Storage.StripeReceivingWays
                .GetAsync(
                    e => 
                    e.OrganizationId == command.ReceiverOrganizationId &&
                    e.PaymentMethod == PaymentMethodType.StripeCard
                );
            if (receivingWay == null)
            {
                Logger.WriteError($"Receiver was not found.");
                return NotFoundResult<PaymentRefundCommandResponse>($"Receiver was not found.");
            }

            var paymentEntity = await Storage.Payments
                .Where(
                    e => 
                    e.Id == command.PaymentId &&
                    e.SendingWay.OrganizationId == command.SenderOrganizationId &&
                    e.SendingWay.ReceivingWay.OrganizationId == command.ReceiverOrganizationId
                )
                .GetAsync();
            if (paymentEntity == null)
            {
                Logger.WriteError($"Payment with id {command.PaymentId} was not found.");
                return NotFoundResult<PaymentRefundCommandResponse>($"Payment with id {command.PaymentId} was not found.");
            }
            if (paymentEntity.Status != PaymentStatusType.Success)
            {
                return ValidationFailResult<PaymentRefundCommandResponse>(
                        nameof(command.PaymentId),
                        "Can not refund payment that has not a success status."
                    );
            }


            using (var transaction = await Storage.BeginTransactionAsync())
            {
                try
                {
                    paymentEntity.Status = PaymentStatusType.Refund;
                    Storage.Payments.Update(paymentEntity);
                    await Storage.SaveChangesAsync();
                    
                    var paymentRefundResult = await StripeApi.SafeCall(
                        api => api.PaymentRefund(receivingWay, paymentEntity)
                    );
                    if (!paymentRefundResult.IsSuccess)
                    {
                        return FailResult<PaymentRefundCommandResponse>(paymentRefundResult);
                    }

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    Logger.WriteError("Transaction fail.");
                    Logger.WriteError(exception);
                    return FailResult<PaymentRefundCommandResponse>("Payment refund was failed. Reason: " + exception.Message);
                }
            }
            
            Logger.WriteInfo("Payment was successfully refund.");
            return SuccessResult(new PaymentRefundCommandResponse
            {
                PaymentId = paymentEntity.Id,
                PaymentStatus = paymentEntity.Status
            });
        }
    }
}
