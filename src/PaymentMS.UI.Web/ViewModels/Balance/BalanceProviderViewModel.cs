using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.BL.Services.Stripe.Models;

namespace PaymentMS.UI.Web.ViewModels.Balance
{
    public class BalanceProviderViewModel : BaseViewModel
    {

        [Display(Name="Stripe Publishable Key")]
        public string StripePublicKey { get; set; }


        [Display(Name="Stripe Secret Key")]
        public string StripeSecretKey { get; set; }

        [Display(Name="Enable Stripe Card Income")]
        public bool IsStripeCardIncomeEnabled { get; set; }

        [Display(Name="Enable Wire Transfer Income")]
        public bool IsWireTransferIncomeEnabled { get; set; }

        [Display(Name = "Receiver Organization Name")]
        [Required]
        [MinLength(3)]
        public string ProviderOrganizationName { get; set; }

        [Display(Name = "Receiver Organization Id")]
        [Required]
        public Guid ProviderOrganizationId { get; set; }

        public List<BalanceProviderOrganizationModel> ReceiverList { get; set; } = new List<BalanceProviderOrganizationModel>();

        public Guid SelectedReceiverId { get; set; }

        public IEnumerable<SelectListItem> CurrencyList { get; set; }

        public string Currency { get; set; }

        public decimal CreditLimit { get; set; }

        public bool HasStipeReceiver { get; set; }

        public bool IsBalanceProvider { get; set; }
    }
}
