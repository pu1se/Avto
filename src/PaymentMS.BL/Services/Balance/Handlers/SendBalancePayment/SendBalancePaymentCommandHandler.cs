using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Balance.Handlers
{
    public class SendBalancePaymentCommandHandler 
        : CommandHandler<SendBalancePaymentCommand, CallResult>
    {
        public SendBalancePaymentCommandHandler(Storage storage, LogService logger) 
            : base(storage, logger)
        {
        }

        protected override async Task<CallResult> HandleCommandAsync(SendBalancePaymentCommand command)
        {
            var client = await Storage.BalanceClients
                .Include(e => e.BalanceProvider)
                .Include(e => e.Payments)
                .GetAsync(
                e => e.OrganizationId == command.ClientOrganizationId &&
                     e.BalanceProvider.OrganizationId == command.ProviderOrganizationId
            );
            if (client == null)
            {
                return NotFoundResult($"Balance client with client organization id {command.ClientOrganizationId} " +
                                      $"and provider organization id {command.ProviderOrganizationId} was not found.");
            }


            var paymentEntity = new PaymentEntity
            {
                PaymentAmount = command.PaymentAmount,
                PaymentCurrency = client.BalanceProvider.Currency,
                PaymentMethod = PaymentMethodType.Balance,
                ExternalId = command.ExternalId,
                ExternalMetadata = JsonConvert.SerializeObject(command.ExternalMetadata),
                Description = command.Description
            };            

            if (client.Amount + client.BalanceProvider.CreditLimit - command.PaymentAmount < 0)
            {
                paymentEntity.Status = PaymentStatusType.Fail;
                paymentEntity.TransactionLog = JsonConvert.SerializeObject(
                    new
                    {
                        ErrorMessage = "Requested payment more than available Credit Limit."
                    }
                );
                client.Payments.Add(paymentEntity);  
                await Storage.SaveChangesAsync();

                return ValidationFailResult(nameof(command.PaymentAmount),
                    "Your current Balance is too low to make a payment.");
            }


            client.Amount -= command.PaymentAmount;
            paymentEntity.Status = PaymentStatusType.Success;
            client.Payments.Add(paymentEntity);  
            Storage.BalanceClients.Update(client);

            using (var transaction = await Storage.BeginTransactionAsync())
            {
                try
                {
                    await Storage.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    return FailResult("Transaction fail: " + ex.Message);
                }
            }
            

            return SuccessResult();
        }
    }
}
