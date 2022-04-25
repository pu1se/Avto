using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Stripe.Api
{
    public class StripeChargeTransactionModel
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
    }
}
