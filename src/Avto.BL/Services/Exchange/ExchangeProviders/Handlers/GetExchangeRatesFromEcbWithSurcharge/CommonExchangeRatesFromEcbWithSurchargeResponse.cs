using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Avto.DAL.Entities;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Currency.ResponseModels
{
    public class CommonExchangeRatesFromEcbWithSurchargeResponse
    {
        public CurrencyType FromCurrency { get; set; }
        public CurrencyType ToCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}
