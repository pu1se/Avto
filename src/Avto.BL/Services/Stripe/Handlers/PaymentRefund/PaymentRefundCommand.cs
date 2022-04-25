using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Stripe.Models
{
    public class PaymentRefundCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid ReceiverOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid SenderOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid PaymentId { get; set; }
    }
}
