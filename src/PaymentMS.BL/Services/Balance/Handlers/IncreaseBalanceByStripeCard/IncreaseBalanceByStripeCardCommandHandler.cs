using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.BL.Services.Stripe.Api;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.BL.Services.Stripe.ResponseModels;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Balance.Handlers
{
    public class IncreaseBalanceByStripeCardCommandHandler 
        : CommandHandler<IncreaseBalanceByStripeCardCommand, CallResult>
    {
        private StripeService StripeService { get; }

        public IncreaseBalanceByStripeCardCommandHandler(
            Storage storage, 
            StripeService stripeService,
            LogService logger) : base(storage, logger)
        {
            StripeService = stripeService;
        }

        protected override async Task<CallResult> HandleCommandAsync(IncreaseBalanceByStripeCardCommand command)
        {
            var client = await Storage.BalanceClients
                .Include(e => e.BalanceProvider)
                .GetAsync(
                    e =>
                        e.OrganizationId == command.ClientOrganizationId &&
                        e.BalanceProvider.OrganizationId == command.ProviderOrganizationId
                );
            if (client == null)
            {
                return NotFoundResult($"Balance client with client organization id {command.ClientOrganizationId} " +
                                      $"and provider organization id {command.ProviderOrganizationId} was not found.");
            }

            if (!client.BalanceProvider.IsStripeCardIncomeEnabled)
            {
                return ValidationFailResult(
                    nameof(command.ProviderOrganizationId),
                    "Increase by Card is not allowed by Balance Provider."
                );
            }


            var increaseBalancePaymentResult = await StripeService.SendPayment(new SendPaymentCommand
            {
                ReceiverOrganizationId = command.ProviderOrganizationId,
                SenderOrganizationId = command.ClientOrganizationId,
                PaymentCurrency = client.BalanceProvider.Currency,
                PaymentAmount = command.AddAmount,
                Description = "Increase Balance by Card",
                PaymentMethod = PaymentMethodType.StripeCardToBalance,
                BalanceClientId = client.Id
            });
            if (!increaseBalancePaymentResult.IsSuccess)
            {
                return increaseBalancePaymentResult;
            }

            using (var transaction = await Storage.BeginTransactionAsync())
            {
                try
                {
                    client.Amount += command.AddAmount;
                    Storage.BalanceClients.Update(client);
                    await Storage.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    var refundPaymentModel = increaseBalancePaymentResult.Data.ToRefundModel();
                    var refundResult = await StripeService.PaymentRefund(refundPaymentModel);
                    if (refundResult.IsSuccess)
                    {
                        Logger.WriteInfo("Payment was successfully refund.");
                    }
                    else
                    {
                        Logger.WriteError($"Payment was not refund. Reason: {refundResult.ErrorMessage}");
                    }
                    
                    return FailResult("Transaction Fail: " + exception.Message);
                }
            }

            return SuccessResult();
        }
    }
}
