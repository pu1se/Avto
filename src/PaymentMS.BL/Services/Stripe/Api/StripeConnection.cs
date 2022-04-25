using System;
using System.Collections.Generic;
using System.Text;
using PaymentMS.DAL.Entities;
using Stripe;

namespace PaymentMS.BL.Services.Stripe.Api
{
    public class StripeConnection
    {
        private IStripeClient StripeClient { get; }

        public StripeConnection(StripeConfigForKeyVault config)
        {
            StripeClient = new StripeClient(config.SecretKey);
        }

        public static StripeConnection For(StripeConfigForKeyVault config)
        {
            return new StripeConnection(config);
        }

        public static StripeConnection For(ReceivingWayEntity receivingWay)
        {
            return new StripeConnection(receivingWay.StripePrivateConfig);
        }

        public CustomerService Customer => new CustomerService(StripeClient);
        public CardService Card => new CardService(StripeClient);
        public ChargeService Charge => new ChargeService(StripeClient);
        public RefundService  Refund => new RefundService(StripeClient);
        public TokenService  Token => new TokenService(StripeClient);
    }
}
