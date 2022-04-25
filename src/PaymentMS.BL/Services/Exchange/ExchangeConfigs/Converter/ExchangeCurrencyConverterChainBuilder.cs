using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;

namespace PaymentMS.BL.Services.Exchange.ExchangeConfigs.Converter
{
    public class ExchangeCurrencyConverterChainBuilder
    {
        private CurrencyType BaseCurrency { get; }
        private IEnumerable<ExchangeConverterInput> ExchangeRateList { get; }
        private decimal FromAmount { get; }
        private CurrencyType FromCurrency { get; }

        public ExchangeCurrencyConverterChainBuilder(CurrencyType baseCurrency,
            IEnumerable<ExchangeConverterInput> exchangeRateList,
            decimal fromAmount,
            CurrencyType fromCurrency)
        {
            BaseCurrency = baseCurrency;
            ExchangeRateList = exchangeRateList;
            FromAmount = fromAmount;
            FromCurrency = fromCurrency;
        }

        public ExchangeRate To(CurrencyType toCurrency)
        {
            decimal fromRate = 1;
            decimal toRate = 1;
            var exchangeDate = DateTime.UtcNow.Date;

            if (FromCurrency != BaseCurrency)
            {
                var exchangeRateEntity = ExchangeRateList.First(
                    e => 
                    e.FromCurrency == BaseCurrency &&
                    e.ToCurrency == FromCurrency
                );
                fromRate = exchangeRateEntity.Rate;
                if (exchangeRateEntity.ExchangeDate < exchangeDate)
                {
                    exchangeDate = exchangeRateEntity.ExchangeDate;
                }
            }

            if (toCurrency != BaseCurrency)
            {
                var exchangeRateEntity = ExchangeRateList.First(
                    e => 
                    e.FromCurrency == BaseCurrency &&
                    e.ToCurrency == toCurrency
                );
                toRate = exchangeRateEntity.Rate;
                if (exchangeRateEntity.ExchangeDate < exchangeDate)
                {
                    exchangeDate = exchangeRateEntity.ExchangeDate;
                }
            }

            var rate = toRate / fromRate;
            if (toCurrency != FromCurrency)
            {
                rate = rate.ToRoundedRate();
            }

            return new ExchangeRate()
            {
                FromCurrency = FromCurrency,
                ToCurrency= toCurrency,
                Rate = rate,
                ExchangeDate = exchangeDate
            };
        }
    }
}
