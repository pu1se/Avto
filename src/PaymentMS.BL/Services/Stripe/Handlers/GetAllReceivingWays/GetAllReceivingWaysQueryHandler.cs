using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Stripe.Handlers.Queries.AllReceivingWays
{
    public class GetAllReceivingWaysQueryHandler : QueryHandler<EmptyQuery, CallListDataResult<ReceivingWayQueryResponse>>
    {
        public GetAllReceivingWaysQueryHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallListDataResult<ReceivingWayQueryResponse>> HandleCommandAsync(EmptyQuery query)
        {
            var entityList = await Storage.StripeReceivingWays
                    .Where(e => e.PaymentMethod == PaymentMethodType.StripeCard)
                    .Include(e => e.Organization)
                    .ToListAsync();
            return SuccessListResult(entityList.Select(e => e.ToReceiverModel()));
        }
    }
}
