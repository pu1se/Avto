using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Balance.Handlers
{
    public class AddBalanceClientCommandHandler 
        : CommandHandler<AddBalanceClientCommand, CallResult>
    {
        public AddBalanceClientCommandHandler(Storage storage, LogService logger) 
            : base(storage, logger)
        {
        }

        protected override async Task<CallResult> HandleCommandAsync(AddBalanceClientCommand command)
        {
            var validationResult = await ValidateAsync(command);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var provider = await Storage.BalanceProviders.GetAsync(
                e => e.OrganizationId == command.ProviderOrganizationId
            );
            if (provider == null)
            {
                return NotFoundResult($"Balance Provider with organization id {command.ProviderOrganizationId} is not exists.");
            }

            try
            {
                await Storage.BalanceClients.AddAsync(new BalanceClientEntity
                {
                    OrganizationId = command.ClientOrganizationId,
                    BalanceProviderId = provider.Id
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return SuccessResult();
        }



        private async Task<CallResult> ValidateAsync(AddBalanceClientCommand command)
        {
            var isClientOrganizationExists = await Storage.Organizations.ExistsAsync(
                e => e.Id == command.ClientOrganizationId
            );
            if (!isClientOrganizationExists)
            {
                return NotFoundResult($"Client Organization with id {command.ClientOrganizationId} is not exists. " +
                                      $"You need to create an organization first.");
            }

            var isClientExists = await Storage.BalanceClients.ExistsAsync(
                e => 
                    e.OrganizationId == command.ClientOrganizationId &&
                    e.BalanceProvider.OrganizationId == command.ProviderOrganizationId
            );
            if (isClientExists)
            {
                return ValidationFailResult(nameof(command.ClientOrganizationId), "Balance Client already exists");
            }

            return SuccessResult();
        }
        
    }
}
