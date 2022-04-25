using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.BL.Services.Stripe.ResponseModels
{
    public class AddPaymentCardCommandResponse
    {
        public string Token { get; set; }
    }
}
