using System;
using System.Collections.Generic;
using System.Text;
using Avto.DAL.Enums;
using Stripe;

namespace Avto.BL.Services.Stripe.Handlers.PaymentRefund
{
    public class PaymentRefundCommandResponse
    {
        public Guid PaymentId { get; set; }
        public PaymentStatusType PaymentStatus { get; set; }
    }
}
