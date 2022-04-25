using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Stripe.Api;
using Avto.BL.Services.Stripe.Models;
using Avto.BL.Services.Stripe.ResponseModels;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Stripe.Handlers.AddPaymentCard
{
    public class AddPaymentCardCommandHandler : CommandHandler<AddPaymentCardCommand, CallDataResult<AddPaymentCardCommandResponse>>
    {
        private SafeCallStripeApi StripeApi { get; }
        public AddPaymentCardCommandHandler(
            Storage storage, 
            SafeCallStripeApi stripeApi,
            LogService logger) 
            : base(storage, logger)
        {
            StripeApi = stripeApi;
        }

        protected override async Task<CallDataResult<AddPaymentCardCommandResponse>> HandleCommandAsync(AddPaymentCardCommand command)
        {
            var receivingWayEntity = await Storage.StripeReceivingWays
                .GetAsync(
                    e =>
                        e.OrganizationId == command.ReceiverOrganizationId &&
                        e.PaymentMethod == PaymentMethodType.StripeCard
                );

            if (receivingWayEntity == null)
            {
                return NotFoundResult<AddPaymentCardCommandResponse>("Receiver wasn't found");
            }

            var callResult = await StripeApi.SafeCall(x => x.AddPaymentCard(receivingWayEntity, command));
            if (!callResult.IsSuccess)
                return FailResult<AddPaymentCardCommandResponse>(callResult);

            return SuccessResult(new AddPaymentCardCommandResponse{Token = callResult.Data});
        }
    }
}
