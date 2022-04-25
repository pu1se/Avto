using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.DAL.Enums;

namespace Avto.UI.Front.Currency
{
    public class ConvertedPriceAndCurrency
    {
        public decimal OriginalAmount { get; set; }
        public CurrencyType OriginalCurrency { get; set; }
        public decimal ConvertedAmount { get; set; }
        public CurrencyType ConvertedCurrency { get; set; }
    }
}
