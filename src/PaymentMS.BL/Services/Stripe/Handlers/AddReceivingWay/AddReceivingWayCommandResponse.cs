using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.BL.Services.Stripe.Handlers.AddReceivingWay
{
    public class AddReceivingWayCommandResponse
    {
        public Guid ReceivingWayId { get; set; }
        public Guid ReceiverOrganizationId { get; set; }
    }
}
