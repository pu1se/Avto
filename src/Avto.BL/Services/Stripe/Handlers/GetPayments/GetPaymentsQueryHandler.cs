using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Stripe.Handlers.Queries.Payments
{
    public class GetPaymentsQueryHandler : QueryHandler<GetPaymentsQuery, CallListDataResult<PaymentQueryResponse>>
    {
        public GetPaymentsQueryHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallListDataResult<PaymentQueryResponse>> HandleCommandAsync(GetPaymentsQuery query)
        {
            var dbRequest = Storage.Payments
                .Where(
                    e => e.SendingWay.OrganizationId == query.SenderOrganizationId &&
                         e.PaymentMethod == PaymentMethodType.StripeCard
                );

            if (query.ReceiverOrganizationId.HasValue)
            {
                dbRequest = dbRequest.Where(
                    e => e.SendingWay.ReceivingWay.OrganizationId == query.ReceiverOrganizationId
                );
            }

            var list = await dbRequest
                .OrderByDescending(x => x.CreatedDateUtc)
                .Select(PaymentQueryResponse.Map())
                .ToListAsync();

            return SuccessListResult(list);
        }
    }
}
