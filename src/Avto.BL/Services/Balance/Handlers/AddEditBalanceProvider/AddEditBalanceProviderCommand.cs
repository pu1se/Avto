using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL.Entities;

namespace Avto.BL.Services.Balance.Models
{
    public class AddEditBalanceProviderCommand : Command
    {
        [NotEmptyValueRequired]
        public Guid ProviderOrganizationId { get; set; }

        [Required]
        public string ProviderOrganizationName { get; set; }

        public bool IsStripeCardIncomeEnabled { get; set; }

        public bool IsWireTransferIncomeEnabled { get; set; }

        [Required]
        public string Currency { get; set; }

        public decimal CreditLimit { get; set; }
    }
}
