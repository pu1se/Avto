using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Stripe.Api;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Stripe.Handlers.Commands.AddEditSendingWay
{
    public class AddEditSendingWayCommandHandler : CommandHandler<AddEditSendingWayCommand, CallDataResult<AddEditSendingWayCommandResponse>>
    {
        private SafeCallStripeApi StripeApi { get; }

        public AddEditSendingWayCommandHandler(Storage storage, SafeCallStripeApi stripeApi, LogService logger) 
            : base(storage, logger)
        {
            StripeApi = stripeApi;
        }

        protected override async Task<CallDataResult<AddEditSendingWayCommandResponse>> HandleCommandAsync(AddEditSendingWayCommand command)
        {
            ReceivingWayEntity receivingWayEntity = receivingWayEntity = await Storage.StripeReceivingWays
                .GetAsync(
                    e => 
                        e.OrganizationId == command.ReceiverOrganizationId &&
                        e.PaymentMethod == PaymentMethodType.StripeCard
                );

            if (receivingWayEntity == null)
            {
                Logger.WriteError("Receiver wasn't found");
                return NotFoundResult<AddEditSendingWayCommandResponse>("Receiver wasn't found");
            }

            var existSendingWayEntityList = new List<SendingWayEntity>();
            var senderOrganization = await CreateOrganizationIfNotExists(command); 
            
            if (senderOrganization.SendingWays.Any())
            {
                existSendingWayEntityList = senderOrganization.SendingWays.Where(
                    e => 
                    e.ReceivingWayId == receivingWayEntity.Id
                ).ToList();
            }


            var cardCreationResult = await StripeApi.SafeCall(
                api => 
                api.SaveDefaultCardAsync(receivingWayEntity, existSendingWayEntityList, command)
            );
            if (!cardCreationResult.IsSuccess)
            {
                Logger.WriteError($"Did not add card to stripe receiver. Reason: {cardCreationResult.ErrorMessage}. Error type: {cardCreationResult.ErrorType}");
                return FailResult<AddEditSendingWayCommandResponse>(cardCreationResult);
            }

            
            foreach (var sendingWay in existSendingWayEntityList)
            {
                sendingWay.IsDefault = false;
                Storage.PaymentSendingWays.Update(sendingWay);
            }
            Logger.WriteInfo($"Set {existSendingWayEntityList.Count()} sending ways to not default.");

            var newSendingWayEntity = new SendingWayEntity
            {
                OrganizationId = command.SenderOrganizationId,
                Configuration = cardCreationResult.Data.ToJson(),
                ReceivingWayId = receivingWayEntity.Id,
                IsDefault = true
            };
            Storage.PaymentSendingWays.Add(newSendingWayEntity);
            await Storage.SaveChangesAsync();

            Logger.WriteInfo($"Successfully sAddOrUpdate default sending way with Id {newSendingWayEntity.Id}.");
            return SuccessResult(newSendingWayEntity.ToSendingWayModel());
        }

        private async Task<OrganizationEntity> CreateOrganizationIfNotExists(AddEditSendingWayCommand model)
        {
            var organization = await Storage.Organizations
                .Include(e => e.SendingWays)
                .GetAsync(
                e =>
                e.Id == model.SenderOrganizationId
            );
            if (organization == null)
            {
                organization = new OrganizationEntity
                {
                    Id = model.SenderOrganizationId,
                    Name = model.SenderOrganizationName
                };
                Storage.Organizations.Add(organization);
                Logger.WriteInfo($"Created organization {model.SenderOrganizationId}.");
            }

            return organization;
        }
    }
}
