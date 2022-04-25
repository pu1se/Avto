using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.AllReceivingWays;
using PaymentMS.BL.Services.Stripe.Models;

namespace PaymentMS.UI.Web.ViewModels
{
    public class AddStripeCardViewModel : BaseViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public List<ReceivingWayQueryResponse> ReceiverList { get; set; }
        public Guid SelectedReceiverId { get; set; }
    }
}
