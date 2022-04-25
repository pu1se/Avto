using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Entities;

namespace PaymentMS.BL.Services.Balance.Models
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
