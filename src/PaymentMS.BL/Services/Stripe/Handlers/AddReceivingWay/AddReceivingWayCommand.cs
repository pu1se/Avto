using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using PaymentMS.BL.Validation;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Stripe.Models
{
    public class AddReceivingWayCommand : Command
    {
        [Required]
        [MinLength(5)]
        public string PublicKey { get; set; }

        [Required]
        [MinLength(5)]
        public string SecretKey { get; set; }

        public string ReceiverOrganizationName { get; set; }

        [NotEmptyValueRequired]
        public Guid ReceiverOrganizationId { get; set; }

        [NotEmptyValueRequired]
        public Guid? ReceivingWayId { get; set; }

        public StripeConfigForKeyVault ToStripePrivateConfiguration()
        {
            return new StripeConfigForKeyVault
            {
                PublicKey = this.PublicKey,
                SecretKey = this.SecretKey
            };
        }

        public StripeConfigForDB ToStripePublicConfiguration()
        {
            return new StripeConfigForDB
            {
                PublicKey = this.PublicKey,
            };
        }
    }    
}
