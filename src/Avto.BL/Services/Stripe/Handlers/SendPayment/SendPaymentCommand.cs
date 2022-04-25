using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Stripe.Models
{
    public class SendPaymentCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid SenderOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid ReceiverOrganizationId { get; set; }

        public Guid ExternalId { get; set; }

        public Dictionary<string, dynamic> ExternalMetadata { get; set; } = new Dictionary<string, dynamic>();

        [MaxLength(2048)]
        public string Description { get; set; }

        [Required]
        public decimal PaymentAmount { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string PaymentCurrency { get; set; }

        internal PaymentMethodType PaymentMethod { get; set; } = PaymentMethodType.StripeCard;
        internal Guid? BalanceClientId { get; set; }
    }
}
