using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay
{
    public class GetDefaultSendingWayQueryHandler : QueryHandler<GetDefaultSendingWayQuery, CallDataResult<DefaultSendingWayQueryResponse>>
    {
        public GetDefaultSendingWayQueryHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallDataResult<DefaultSendingWayQueryResponse>> HandleCommandAsync(GetDefaultSendingWayQuery query)
        {
            var defaultSendingWay = await Storage.PaymentSendingWays
                .Where(
                    e => e.OrganizationId == query.SenderOrganizationId &&
                         e.IsDefault &&
                         e.ReceivingWay.OrganizationId == query.ReceiverOrganizationId &&
                         e.ReceivingWay.PaymentMethod == PaymentMethodType.StripeCard
                )
                .OrderByDescending(e => e.CreatedDateUtc)
                .Select(entity => new DefaultSendingWayQueryResponse
                {
                    Id = entity.Id,
                    SenderOrganizationId = entity.OrganizationId,
                    ReceiverOrganizationId = entity.ReceivingWay.OrganizationId,
                    ReceivingWayId = entity.ReceivingWay.Id,
                    Card = entity.Configuration.ToStripeCard()
                })
                .GetAsync();

            if (defaultSendingWay == null)
                return new NotFoundResult<DefaultSendingWayQueryResponse>("Default payment sending way wasn't found");

            return SuccessResult(defaultSendingWay);
        }
    }
}
