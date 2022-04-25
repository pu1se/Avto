using System.Collections.Generic;
using System.Linq;
using Avto.DAL.Entities;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange.CurrencyConverter
{
    public class ExchangeCurrencyChainBuilder
    {
        private CurrencyType BaseCurrency { get; }
        private IEnumerable<CurrencyExchangeRateEntity> ExchangeRateEntities { get; }
        private decimal FromAmount { get; }
        private CurrencyType FromCurrency { get; }
        private CurrencyConfiguration CurrencyConfiguration { get; }
        private Dictionary<CurrencyType, decimal> CurrencyDictWithSpecificSurcharge { get; }

        public ExchangeCurrencyChainBuilder(
            CurrencyType baseCurrency, 
            IEnumerable<CurrencyExchangeRateEntity> exchangeRateEntities, 
            CurrencyConfiguration currencyConfiguration,
            decimal fromAmount, 
            CurrencyType fromCurrency)
        {
            BaseCurrency = baseCurrency;
            ExchangeRateEntities = exchangeRateEntities;
            FromAmount = fromAmount;
            FromCurrency = fromCurrency;
            CurrencyConfiguration = currencyConfiguration;
            CurrencyDictWithSpecificSurcharge = GetSurchargeDict();
        }

        public ExchangedPriceAndCurrency To(CurrencyType toCurrency)
        {
            decimal fromRate = 1;
            decimal toRate = 1;

            if (FromCurrency != BaseCurrency)
            {
                var exchangeRateEntity = ExchangeRateEntities.First(e => e.ToCurrencyCode == FromCurrency.ToString());
                fromRate = exchangeRateEntity.Rate;
            }

            if (toCurrency != BaseCurrency)
            {
                var exchangeRateEntity = ExchangeRateEntities.First(e => e.ToCurrencyCode == toCurrency.ToString());
                toRate = exchangeRateEntity.Rate;
            }

            var rate = toRate / fromRate;
            if (toCurrency != FromCurrency)
            {
                var surchargeMultiplier = 1 + GetRiskSurchargeAsPercent(toCurrency, FromCurrency) / 100;
                rate *= surchargeMultiplier;
                rate = rate.ToRoundedRate();
            }

            return new ExchangedPriceAndCurrency
            {
                FromAmount = FromAmount,
                FromCurrency = FromCurrency,
                ExchangeCurrency = toCurrency,
                ExchangeRate = rate,
            };
        }

        private Dictionary<CurrencyType, decimal> GetSurchargeDict()
        {
            return new Dictionary<CurrencyType, decimal>
            {
                {CurrencyType.USD, CurrencyConfiguration.ExchangeRiskSurchargeAsPercentForUSD},
                {CurrencyType.SEK, CurrencyConfiguration.ExchangeRiskSurchargeAsPercentForSEK},
                {CurrencyType.DKK, CurrencyConfiguration.ExchangeRiskSurchargeAsPercentForDKK},
                {CurrencyType.NOK, CurrencyConfiguration.ExchangeRiskSurchargeAsPercentForNOK}
            };
        }

        private decimal GetRiskSurchargeAsPercent(params CurrencyType[] convertingCurrencies)
        {
            var specificSurcharge = CurrencyDictWithSpecificSurcharge
                .Where(x => convertingCurrencies.Contains(x.Key))
                .OrderByDescending(x => x.Value);
            if (specificSurcharge.Any())
            {
                return specificSurcharge.First().Value;
            }

            return CurrencyConfiguration.ExchangeRiskSurchargeAsPercent;
        }
    }
}
