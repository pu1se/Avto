using System;
using System.Collections.Generic;
using System.Linq;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.Converter
{
    public class ExchangeCurrencyConverter
    {
        private CurrencyType BaseCurrency { get; }
        private List<ExchangeConverterInput> ExchangeRateList { get; }
        private decimal FromAmount { get; }

        public ExchangeCurrencyConverter(CurrencyType baseCurrency, List<ExchangeConverterInput> exchangeRateList)
        {
            if (exchangeRateList == null || !exchangeRateList.Any())
            {
                throw new ArgumentException($"{nameof(exchangeRateList)} can not be empty.");
            }

            BaseCurrency = baseCurrency;
            ExchangeRateList = exchangeRateList;
            FromAmount = 1;
        }

        public ExchangeCurrencyConverterChainBuilder From(CurrencyType fromCurrency)
        {
            return new ExchangeCurrencyConverterChainBuilder(BaseCurrency, ExchangeRateList, FromAmount, fromCurrency);
        } 
    }
}
