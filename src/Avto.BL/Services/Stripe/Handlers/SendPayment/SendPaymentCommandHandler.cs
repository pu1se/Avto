using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Avto.BL.Services.Stripe.Api;
using Avto.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using Avto.BL.Services.Stripe.Models;
using Avto.BL.Services.Stripe.ResponseModels;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Stripe.Handlers
{
    public class SendPaymentCommandHandler : CommandHandler<SendPaymentCommand, CallDataResult<SendPaymentCommandResponse>>
    {
        private SafeCallStripeApi StripeApi { get; }
        private GetDefaultSendingWayQueryHandler GetDefaultSendingWayHandler { get; }
        private PaymentRefundCommandHandler PaymentRefundHandler { get; }

        public SendPaymentCommandHandler(
            SafeCallStripeApi stripeApi, 
            Storage storage, 
            GetDefaultSendingWayQueryHandler getDefaultSendingWayHandler,
            PaymentRefundCommandHandler paymentRefundHandler,
            LogService logger) 
            : base(storage, logger)
        {
            StripeApi = stripeApi;
            GetDefaultSendingWayHandler = getDefaultSendingWayHandler;
            PaymentRefundHandler = paymentRefundHandler;
        }

        protected override async Task<CallDataResult<SendPaymentCommandResponse>> HandleCommandAsync(SendPaymentCommand command)
        {
            command.PaymentCurrency = command.PaymentCurrency.ToUpper();

            var receivingWay = await Storage.StripeReceivingWays.GetAsync(
                e=>
                e.OrganizationId == command.ReceiverOrganizationId &&
                e.PaymentMethod == PaymentMethodType.StripeCard
            );
            if (receivingWay == null)
            {
                Logger.WriteError("Receiver wasn't found");
                return NotFoundResult<SendPaymentCommandResponse>("Receiver wasn't found");
            }

            var getSendingWayResult = await GetDefaultSendingWayHandler.HandleAsync(new GetDefaultSendingWayQuery
            {
                SenderOrganizationId = command.SenderOrganizationId, 
                ReceiverOrganizationId = command.ReceiverOrganizationId
            });

            if (!getSendingWayResult.IsSuccess)
            {
                Logger.WriteError($"Stripe default sending way sender organizationId {command.SenderOrganizationId} and " +
                                  $"receiver organizationId {command.ReceiverOrganizationId} was not found.");
                return FailResult<SendPaymentCommandResponse>(getSendingWayResult);
            }

            var paymentEntity = new PaymentEntity
            {
                Id = Guid.NewGuid(),
                SendingWayId = getSendingWayResult.Data.Id,
                PaymentAmount = command.PaymentAmount,
                PaymentCurrency = command.PaymentCurrency,
                Description = command.Description,
                ExternalId = command.ExternalId,
                ExternalMetadata = JsonConvert.SerializeObject(command.ExternalMetadata),
                PaymentMethod = command.PaymentMethod,
                BalanceClientId = command.BalanceClientId
            };

            Logger.WriteInfo($"Try to send payment {paymentEntity.Id} to stripe, " +
                             $"from sender organizationId {command.SenderOrganizationId} to " +
                             $"receiver organizationId {command.ReceiverOrganizationId}." +
                             $"payment amount {command.PaymentAmount} currency {command.PaymentCurrency}.");
            var paymentResult = await StripeApi.SafeCall(
                api=>
                api.SendPayment(getSendingWayResult.Data.Card, receivingWay, command)
            );
            if (!paymentResult.IsSuccess)
            {
                Logger.WriteError($"Payment was not success. Reason {paymentResult.ErrorMessage}. Error type {paymentResult.ErrorType}");
                paymentEntity.TransactionLog = JsonConvert.SerializeObject(
                    new
                    {
                        ErrorMessage = paymentResult.ErrorMessage
                    }
                );
                paymentEntity.Status = PaymentStatusType.Fail;
                await Storage.Payments.AddAsync(paymentEntity);
                return FailResult<SendPaymentCommandResponse>(paymentResult);
            }

            using (var transaction = await Storage.BeginTransactionAsync())
            {
                try
                {
                    paymentEntity.TransactionLog = paymentResult.Data;
                    paymentEntity.Status = PaymentStatusType.Success;
                    await Storage.Payments.AddAsync(paymentEntity);
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    Logger.WriteError("Transaction fail.");
                    Logger.WriteError(exception);
                    await RefundPayment(receivingWay, paymentResult);
                    return FailResult<SendPaymentCommandResponse>(exception.Message);
                }
            }
            
            Logger.WriteInfo("Payment successfully done.");
            return SuccessResult(new SendPaymentCommandResponse
            {
                ReceiverOrganizationId = command.ReceiverOrganizationId,
                SenderOrganizationId = command.SenderOrganizationId,
                PaymentId = paymentEntity.Id,
                PaymentStatus = paymentEntity.Status
            });
        }

        private async Task RefundPayment(ReceivingWayEntity receivingWay, CallDataResult<string> paymentResult)
        {
            Logger.WriteInfo($"Try to refund stripe payment {paymentResult.Data}.");
            var stripeCharge = StripePayment.DeserializePaymentJson(paymentResult.Data);
            if (stripeCharge == null)
            {
                return;
            }

            for (var i = 0; i < 10000; i++)
            {
                var refundResult = await StripeApi.SafeCall(
                    api => api.PaymentRefund(receivingWay, stripeCharge)
                );

                if (refundResult.IsSuccess)
                {
                    Logger.WriteInfo("Payment refund success.");
                    break;
                }
                Logger.WriteError($"Payment refund fail. Reason: {refundResult.ErrorMessage}.");
                await Task.Delay(5000);
            }
        }
    }
}
