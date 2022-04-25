using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Stripe.Handlers.Queries.Payments
{
    public class GetPaymentsQuery : Query
    {
        [NotEmptyValueRequired]
        public Guid SenderOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid? ReceiverOrganizationId { get; set; }
    }
}
