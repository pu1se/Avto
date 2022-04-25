using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.DAL.Enums;
using PaymentMS.UI.Front.Currency;

namespace PaymentMS.UI.Front.ViewModels
{
    public class PricesListPageModel : BasePageModel
    {
        public List<CurrencyType> CurrencyList { get; set; }

        public CurrencyConverter CurrencyConverter { get; set; }

        public string SelectedCurrency { get; set; }
    }
}
