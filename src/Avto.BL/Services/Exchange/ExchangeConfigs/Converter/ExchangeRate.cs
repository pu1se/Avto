using System;
using System.Collections.Generic;
using System.Text;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.ExchangeConfigs.Converter
{
    public class ExchangeRate
    {
        public CurrencyType FromCurrency { get; set; }
        public decimal Rate { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}
