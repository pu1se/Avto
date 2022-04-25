using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentMS.BL.Services.Stripe.Handlers.AddPaymentCard
{
    public class AddPaymentCardCommand : Command
    {
        [Required]
        public string Number { get; set; }

        [Range(1, 12)]
        public int ExpMonth { get; set; }

        [Range(20, 40)]
        public int ExpYear { get; set; }

        [Required]
        public string Cvc { get; set; }

        [Required]
        public string CardholderName { get; set; }

        [NotEmptyValueRequired]
        public Guid ReceiverOrganizationId { get; set; }
    }
}
