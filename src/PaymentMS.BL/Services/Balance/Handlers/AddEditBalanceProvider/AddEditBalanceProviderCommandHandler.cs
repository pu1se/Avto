using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Balance.Handlers
{
    public class AddEditBalanceProviderCommandHandler 
        : CommandHandler<AddEditBalanceProviderCommand, CallResult>
    {
        public AddEditBalanceProviderCommandHandler(
            Storage storage, 
            LogService logger) 
            : base(storage, logger)
        {
        }

        protected override async Task<CallResult> HandleCommandAsync(AddEditBalanceProviderCommand command)
        {
            if (!command.IsStripeCardIncomeEnabled && !command.IsWireTransferIncomeEnabled)
            {
                var errorMessage = "At least one of income ways must be selected";
                var errors = new Dictionary<string, string>
                {
                    {nameof(command.IsWireTransferIncomeEnabled), errorMessage},
                    {nameof(command.IsStripeCardIncomeEnabled), errorMessage}
                };
                return ValidationFailResult(errors);
            }

            var organization = await CreateOrganizationIfNotExists(command);
            if (command.IsStripeCardIncomeEnabled && organization.ReceivingWays.Count == 0)
            {
                return ValidationFailResult(
                    nameof(command.IsStripeCardIncomeEnabled),
                    "If Stripe income is enabled, then it must be configured first.");
            }

            var balanceProvider = organization.BalanceProviders.FirstOrDefault() ?? new BalanceProviderEntity();
            balanceProvider.CreditLimit = command.CreditLimit;
            balanceProvider.Currency = command.Currency;
            balanceProvider.IsStripeCardIncomeEnabled = command.IsStripeCardIncomeEnabled;
            balanceProvider.IsWireTransferIncomeEnabled = command.IsWireTransferIncomeEnabled;
            balanceProvider.OrganizationId = command.ProviderOrganizationId;

            if (organization.BalanceProviders.Count == 0)
            {
                Storage.BalanceProviders.Add(balanceProvider);
            }
            else
            {
                Storage.BalanceProviders.Update(balanceProvider);
            }

            if (balanceProvider.IsStripeCardIncomeEnabled)
            {
                await AttachReceivingWay(balanceProvider);
            }

            await Storage.SaveChangesAsync();

            return SuccessResult();
        }
        

        private async Task AttachReceivingWay(BalanceProviderEntity balanceProvider)
        {            
            var receivingWays = await Storage.StripeReceivingWays.Where(
                e => 
                e.OrganizationId == balanceProvider.OrganizationId
            ).ToListAsync();

            foreach (var receivingWay in receivingWays)
            {
                receivingWay.BalanceProviderId = balanceProvider.Id;
                Storage.StripeReceivingWays.Update(receivingWay);
            }
            
        }

        private async Task<OrganizationEntity> CreateOrganizationIfNotExists(AddEditBalanceProviderCommand command)
        {
            var organization = await Storage.Organizations
                .Include(e => e.ReceivingWays)
                .Include(e => e.BalanceProviders)
                .GetAsync(e => e.Id == command.ProviderOrganizationId);
            if (organization == null)
            {
                organization = new OrganizationEntity
                {
                    Id = command.ProviderOrganizationId,
                    Name = command.ProviderOrganizationName
                };
                await Storage.Organizations.AddAsync(organization);
            }

            return organization;
        }
    }
}
