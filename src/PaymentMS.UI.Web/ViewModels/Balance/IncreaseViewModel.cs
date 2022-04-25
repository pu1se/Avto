using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using PaymentMS.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.DAL.Enums;

namespace PaymentMS.UI.Web.ViewModels.Balance
{
    public class IncreaseViewModel : BaseViewModel
    {
        public string Currency { get; set; }

        public decimal CurrentAmount { get; set; }

        public decimal AddAmount { get; set; }

        public IEnumerable<SelectListItem> CurrencyList { get; set; }
        public DefaultSendingWayQueryResponse CurrentCard { get; set; }
    }
}
