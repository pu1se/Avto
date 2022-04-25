using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PaymentMS.BL.Validation;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Stripe.Models
{
    public class AddEditSendingWayCommand : Command
    {
        [Required]
        public string Token { get; set; }

        [NotEmptyValueRequired]
        public Guid SenderOrganizationId { get; set; }

        [Required]
        public string SenderOrganizationName { get; set; }

        [Required]
        public string SenderEmail { get; set; }

        [Required]
        public string SenderName { get; set; }

        [NotEmptyValueRequired]
        public Guid ReceiverOrganizationId { get; set; }
    }
}
