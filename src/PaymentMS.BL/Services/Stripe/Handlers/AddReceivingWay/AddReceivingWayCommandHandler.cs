using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Stripe.Api;
using PaymentMS.BL.Services.Stripe.Handlers.AddReceivingWay;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Stripe.Handlers
{
    public class AddReceivingWayCommandHandler : CommandHandler<AddReceivingWayCommand, CallDataResult<AddReceivingWayCommandResponse>>
    {
        private SafeCallStripeApi StripeApi { get; }

        public AddReceivingWayCommandHandler(
            Storage storage, 
            SafeCallStripeApi stripeApi, 
            LogService logger) 
            : base(storage, logger)
        {
            StripeApi = stripeApi;
        }

        protected override async Task<CallDataResult<AddReceivingWayCommandResponse>> HandleCommandAsync(AddReceivingWayCommand command)
        {
            var validationResult = await ValidateAsync(command);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            await CreateOrganizationIfNotExists(command);

            var newReceivingWay = new ReceivingWayEntity
            {
                Id = command.ReceivingWayId ?? Guid.NewGuid(),
                OrganizationId = command.ReceiverOrganizationId,
                StripePrivateConfig = command.ToStripePrivateConfiguration(),
                StripePublicConfig = command.ToStripePublicConfiguration(),
                PaymentMethod = PaymentMethodType.StripeCard
            };
            await Storage.StripeReceivingWays.AddAsync(newReceivingWay);
            await Storage.SaveChangesAsync();

            return SuccessResult(new AddReceivingWayCommandResponse
            {
                ReceivingWayId = newReceivingWay.Id,
                ReceiverOrganizationId = newReceivingWay.OrganizationId
            });
        }

        private async Task CreateOrganizationIfNotExists(AddReceivingWayCommand model)
        {
            var organizationExists = await Storage.Organizations.ExistsAsync(
                            e =>
                            e.Id == model.ReceiverOrganizationId
                        );
            if (!organizationExists)
            {
                Storage.Organizations.Add(new OrganizationEntity
                {
                    Id = model.ReceiverOrganizationId,
                    Name = model.ReceiverOrganizationName
                });
                Logger.WriteInfo($"Created organization {model.ReceiverOrganizationName}.");
            }
        }



        private async Task<CallDataResult<AddReceivingWayCommandResponse>> ValidateAsync(AddReceivingWayCommand command)
        {
            var existReceivingWay = await Storage.StripeReceivingWays.GetAsync(
                e =>
                    e.OrganizationId == command.ReceiverOrganizationId &&
                    e.PaymentMethod == PaymentMethodType.StripeCard
            );

            if (existReceivingWay != null)
            {
                if (
                    existReceivingWay.StripePublicConfig.PublicKey != command.PublicKey ||
                    existReceivingWay.StripePrivateConfig.PublicKey != command.PublicKey ||
                    existReceivingWay.StripePrivateConfig.SecretKey != command.SecretKey)
                {
                    return ValidationFailResult<AddReceivingWayCommandResponse>(
                        nameof(command.ReceiverOrganizationId),
                        "Organization already has Stripe receiving way. We don't support stripe keys update right now."
                    );
                }
                else
                {
                    return ValidationFailResult<AddReceivingWayCommandResponse>(nameof(command.ReceiverOrganizationId), "Stripe receiving way already exists");
                }
            }

            if (!command.IsValid())
            {
                return ValidationFailResult<AddReceivingWayCommandResponse>(command);
            }

            var credentialsValidationResult = await StripeApi.SafeCall(
                api => api.ValidateReceiverCredentials(command.ToStripePrivateConfiguration())
            );
            if (!credentialsValidationResult.IsSuccess)
            {
                return ValidationFailResult<AddReceivingWayCommandResponse>(nameof(command.SecretKey), credentialsValidationResult.ErrorMessage);
            }            

            return ValidationSuccessResult<AddReceivingWayCommandResponse>();
        }
    }
}
