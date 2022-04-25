using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.AllReceivingWays;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.Payments;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Enums;

namespace PaymentMS.UI.Web.ViewModels
{
    public class PayWithStripeCard : BaseViewModel
    {
        public List<ReceivingWayQueryResponse> ReceiverList { get; set; }
        public Guid SelectedReceivingWayId { get; set; }
        public decimal PaymentAmount { get; set; }
        public List<PaymentQueryResponse> PaymentList { get; set; }
        public IEnumerable<SelectListItem> CurrencyList { get; set; }
        public string PaymentCurrency { get; set; }
    }
}
