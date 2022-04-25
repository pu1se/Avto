using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Avto.BL.Validation;
using Avto.DAL.Entities;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Stripe.Models
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
