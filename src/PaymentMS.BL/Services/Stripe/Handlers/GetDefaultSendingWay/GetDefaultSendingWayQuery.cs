using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay
{
    public class GetDefaultSendingWayQuery : Query
    {
        [NotEmptyValueRequired]
        public Guid SenderOrganizationId { get; set; } 

        [NotEmptyValueRequired]
        public Guid ReceiverOrganizationId { get; set; }
    }
}
