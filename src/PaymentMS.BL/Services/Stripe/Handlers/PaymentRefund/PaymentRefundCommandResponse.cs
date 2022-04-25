using System;
using System.Collections.Generic;
using System.Text;
using PaymentMS.DAL.Enums;
using Stripe;

namespace PaymentMS.BL.Services.Stripe.Handlers.PaymentRefund
{
    public class PaymentRefundCommandResponse
    {
        public Guid PaymentId { get; set; }
        public PaymentStatusType PaymentStatus { get; set; }
    }
}
