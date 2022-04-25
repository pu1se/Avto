using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaymentMS.UI.Web.ViewModels.Balance
{
    public class BalancePayViewModel : BaseViewModel
    {
        public string Currency { get; set; }

        public decimal CreditLimit { get; set; }

        public decimal CurrentAmount { get; set; }

        public decimal PaymentAmount { get; set; }

        public IEnumerable<SelectListItem> CurrencyList { get; set; }
    }
}
