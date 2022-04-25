using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.AllReceivingWays;
using PaymentMS.BL.Services.Stripe.Models;
using RestSharp.Validation;

namespace PaymentMS.UI.Web.ViewModels
{
    public class AddStripeReceiverViewModel : BaseViewModel
    {
        [Required]
        [MinLength(5)]
        public string PublicKey { get; set; }

        [Required]
        [MinLength(5)]
        public string SecretKey { get; set; }

        [Required]
        [MinLength(3)]
        public string ReceiverOrganizationName { get; set; }

        [Required]
        public Guid ReceiverOrganizationId { get; set; }

        public List<ReceivingWayQueryResponse> ReceiverList { get; set; }
    }
}
