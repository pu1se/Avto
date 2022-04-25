using System;
using System.Collections.Generic;
using System.Text;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Exchange.ExchangeConfigs.Converter
{
    public class ExchangeConverterInput
    {
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}
