using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Balance.Handlers
{
    public class IncreaseBalanceByWireTransferCommandHandler 
        : CommandHandler<IncreaseBalaceByWireTransferCommand, CallResult>
    {
        public IncreaseBalanceByWireTransferCommandHandler(Storage storage, LogService logger) 
            : base(storage, logger)
        {
        }

        protected override async Task<CallResult> HandleCommandAsync(IncreaseBalaceByWireTransferCommand command)
        {
            var client = await Storage.BalanceClients.Where(
                e => 
                e.OrganizationId == command.ClientOrganizationId &&
                e.BalanceProvider.OrganizationId == command.ProviderOrganizationId
                )
                .Include(e => e.BalanceProvider)            
                .Include(e => e.Payments)
                .GetAsync();
            if (client == null)
            {
                return NotFoundResult($"Balance client with client organization id {command.ClientOrganizationId} " +
                                      $"and provider organization id {command.ProviderOrganizationId} was not found.");
            }
            if (!client.BalanceProvider.IsWireTransferIncomeEnabled)
            {
                return ValidationFailResult(
                    nameof(command.ProviderOrganizationId),
                    "Increase by Wire Transfer is not allowed by Balance Provider."
                );
            }

            client.Amount += command.AddAmount;
            client.Payments.Add(new PaymentEntity
            {
                PaymentAmount = command.AddAmount,
                PaymentCurrency = client.BalanceProvider.Currency,
                Status = PaymentStatusType.Success,
                PaymentMethod = PaymentMethodType.WireTransferToBalance,
                Description = "Increase Balance by Wire transfer",
            });
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
