using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentMS.UI.Front.ViewModels
{
    public class OfferItemModel
    {
        public string Name { get; set; }
        public Currency.ConvertedPriceAndCurrency Price { get; set; }
    }
}
