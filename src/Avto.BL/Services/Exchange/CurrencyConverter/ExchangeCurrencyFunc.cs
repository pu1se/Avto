﻿using System.Collections.Generic;
using Avto.DAL.Entities;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.CurrencyConverter
{
    public class ExchangeCurrencyFunc
    {
        private CurrencyType BaseCurrency { get; }
        private IEnumerable<CurrencyExchangeRateEntity> ExchangeRateEntities { get; }
        private CurrencyConfiguration CurrencyConfiguration { get; }

        public ExchangeCurrencyFunc(CurrencyType baseCurrency,
            IEnumerable<CurrencyExchangeRateEntity> exchangeRateEntities,
            CurrencyConfiguration currencyConfiguration)
        {
            BaseCurrency = baseCurrency;
            ExchangeRateEntities = exchangeRateEntities;
            CurrencyConfiguration = currencyConfiguration;
        }

        public ExchangeCurrencyChainBuilder From(decimal fromAmount, CurrencyType fromCurrency)
        {
            return new ExchangeCurrencyChainBuilder(BaseCurrency, ExchangeRateEntities, CurrencyConfiguration, fromAmount, fromCurrency);
        } 
    }
}