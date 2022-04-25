using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.DAL.Enums;
using Avto.UI.Front.Currency;

namespace Avto.UI.Front.ViewModels
{
    public class PricesListPageModel : BasePageModel
    {
        public List<CurrencyType> CurrencyList { get; set; }

        public CurrencyConverter CurrencyConverter { get; set; }

        public string SelectedCurrency { get; set; }
    }
}
