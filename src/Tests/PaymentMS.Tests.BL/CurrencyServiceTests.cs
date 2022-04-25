using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.BL.Services.Currency;
using PaymentMS.BL.Services.Exchange.Currency;
using PaymentMS.BL.Services.Exchange.CurrencyConverter;
using PaymentMS.BL.Services.Exchange.ExchangeProviders;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.API;
using PaymentMS.DAL.Enums;
using PaymentMS.Tests.BL.Base;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class CurrencyServiceTests : BaseServiceTests<CurrencyService>
    {
        [TestInitialize]
        public void Init()
        {
            var currentDate = DateTime.UtcNow.Date;
            var exchangeRates = Storage.CurrencyExchangeRates
                .Where(e => e.ExchangeDate == currentDate)
                .ToListAsync().GetAwaiter().GetResult();
            if (!exchangeRates.Any())
            {
                var exchangeProviderService = Resolve<ExchangeProviderService>();
                exchangeProviderService.RefreshTodayRatesForProvider(ExchangeProviderType.ECB).GetAwaiter().GetResult();
            }
        }

        [TestMethod]
        public async Task SuccessExchangeCurrencyToBaseCurrency()
        {
            var settings = Resolve<AppSettings>();
            var baseCurrency = GetBaseCurrency();
            var exchangeRateEntities = await GetLatestExchangeRateFromDB();
            var exchangeCurrency = new ExchangeCurrencyFunc(
                baseCurrency, 
                exchangeRateEntities, 
                settings.Currency);

            var currencyList = GetSupportedCurrencyList();

            foreach (var fromCurrency in currencyList)
            {
                var convertionResult = exchangeCurrency.From(1, fromCurrency).To(baseCurrency);
                var exchangeRate = exchangeRateEntities.First(e => e.ToCurrencyCode == fromCurrency.ToString());
                var surchargeAsPercent = GetRiskSurchargeAsPercent(settings.Currency, fromCurrency, baseCurrency);
                var calcRateValue = exchangeRate.Rate;
                if (fromCurrency != baseCurrency)
                {
                    calcRateValue = (1 / exchangeRate.Rate * (1 + surchargeAsPercent / 100));
                }
                calcRateValue = calcRateValue.ToRoundedRate();
                Assert.IsTrue(convertionResult.FromCurrency == fromCurrency);
                Assert.IsTrue(convertionResult.ExchangeCurrency == baseCurrency);
                Assert.IsTrue(convertionResult.FromAmount == 1);
                Assert.IsTrue(convertionResult.ExchangeRate == calcRateValue);
            }
        }

        [TestMethod]
        public async Task SuccessExchangeCurrencyFromBaseCurrency()
        {
            var settings = Resolve<AppSettings>();
            var baseCurrency = GetBaseCurrency();
            var exchangeRateEntities = await GetLatestExchangeRateFromDB();
            var exchangeCurrency = new ExchangeCurrencyFunc(baseCurrency, exchangeRateEntities, settings.Currency);

            var currencyList = GetSupportedCurrencyList();

            foreach (var toCurrency in currencyList)
            {
                var convertionResult = exchangeCurrency.From(1, baseCurrency).To(toCurrency);
                var exchangeRateEntity = exchangeRateEntities.First(e => e.ToCurrencyCode == toCurrency.ToString());
                Assert.IsTrue(convertionResult.FromCurrency == baseCurrency);
                Assert.IsTrue(convertionResult.ExchangeCurrency == toCurrency);
                Assert.IsTrue(convertionResult.FromAmount == 1);
                var surchargeAsPercent = GetRiskSurchargeAsPercent(settings.Currency, baseCurrency, toCurrency);

                var calcExchangeRate = exchangeRateEntity.Rate;
                if (toCurrency != baseCurrency)
                {
                    calcExchangeRate = exchangeRateEntity.Rate *
                                           (1 + surchargeAsPercent / 100);
                }

                Assert.IsTrue(convertionResult.ExchangeRate == calcExchangeRate);
            }
        }

        [TestMethod]
        public async Task SuccessCheckThatSupportedCurrenciesEquelsToCurrencyEnum()
        {
            var currenciesFromDB = await Storage.Currencies.ToListAsync();
            Assert.IsTrue(currenciesFromDB.Any());

            var enumCurrencies = EnumHelper.ToList<CurrencyType>();
            Assert.IsTrue(enumCurrencies.Any());

            Assert.IsTrue(enumCurrencies.Count() == currenciesFromDB.Count);
            var differenceBetweenDbAndEnum = currenciesFromDB
                .Select(e => e.Code)
                .Except(enumCurrencies.Select(x => x.ToString()));
            Assert.IsTrue(!differenceBetweenDbAndEnum.Any());
        }

        [TestMethod]
        public async Task SuccessRefreshCurrencyAndExchangeRateTables()
        {
            var baseCurrency = GetBaseCurrency();

            var currencyList = GetSupportedCurrencyList();
            List<DAL.Entities.CurrencyExchangeRateEntity> exchangeRate = await GetLatestExchangeRateFromDB();
            foreach (var currency in currencyList)
            {
                if (currency == baseCurrency)
                {
                    continue;
                }

                var rate = exchangeRate.FirstOrDefault(e => e.ToCurrencyCode == currency.ToString());
                Assert.IsTrue(rate != null);
                Assert.IsTrue(rate.Rate > 0);
                Assert.IsTrue(rate.LastUpdatedDateUtc > DateTime.UtcNow.Date);
            }
        }

        [TestMethod]
        public async Task GetAvailableCurrencies()
        {
            var availableCurrenciesResult = await Service.GetAvailableCurrencies();
            Assert.IsTrue(availableCurrenciesResult.IsSuccess);

            foreach (var currency in availableCurrenciesResult.Data)
            {
                Assert.IsTrue(currency.Code.IsNullOrEmpty() == false);
                Assert.IsTrue(currency.Name.IsNullOrEmpty() == false);
            }
        }

        private async Task<List<DAL.Entities.CurrencyExchangeRateEntity>> GetLatestExchangeRateFromDB()
        {
            var currentDate = DateTime.UtcNow.Date;
            var exchangeRate = await Storage.CurrencyExchangeRates
                .Where(e => e.ExchangeDate == currentDate)
                .OrderByDescending(e => e.ExchangeDate)
                .ToListAsync();
            return exchangeRate;
        }

        private decimal GetRiskSurchargeAsPercent(CurrencyConfiguration currencyConfiguration, CurrencyType fromCurrency, CurrencyType toCurrency)
        {
            var dict = new Dictionary<CurrencyType, decimal>
            {
                {CurrencyType.USD, currencyConfiguration.ExchangeRiskSurchargeAsPercentForUSD},
                {CurrencyType.SEK, currencyConfiguration.ExchangeRiskSurchargeAsPercentForSEK},
                {CurrencyType.DKK, currencyConfiguration.ExchangeRiskSurchargeAsPercentForDKK},
                {CurrencyType.NOK, currencyConfiguration.ExchangeRiskSurchargeAsPercentForNOK}
            };

            if (dict.ContainsKey(fromCurrency) | dict.ContainsKey(toCurrency))
            {
                var surcharge = dict
                    .Where(x => x.Key == toCurrency || x.Key == fromCurrency)
                    .OrderByDescending(x => x.Value)
                    .First();
                return surcharge.Value;
            }

            return currencyConfiguration.ExchangeRiskSurchargeAsPercent;
        }

        private CurrencyType GetBaseCurrency()
        {
            var ecbApi = Resolve<EcbProviderApi>();
            return ecbApi.GetBaseCurrency();
        }

        private List<CurrencyType> GetSupportedCurrencyList()
        {
            var ecbApi = Resolve<EcbProviderApi>();
            return ecbApi.GetSupportedCurrencies();
        }
    }
}
