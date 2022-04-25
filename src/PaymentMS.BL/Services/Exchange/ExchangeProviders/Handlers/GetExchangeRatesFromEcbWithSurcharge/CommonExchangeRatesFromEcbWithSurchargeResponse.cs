using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Currency.ResponseModels
{
    public class CommonExchangeRatesFromEcbWithSurchargeResponse
    {
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}
