using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange;
using Avto.DAL.Enums;
using Avto.Tests.BL.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avto.Tests.BL
{
    [TestClass]
    public class ExchangeServiceTests : BaseServiceTests<ExchangeService>
    {
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
            var currencyList = EnumHelper.ToList<CurrencyType>();
            var currentDate = DateTime.UtcNow.Date;
            var exchangeRate = await Storage.ExchangeRates
                .Where(e => e.ExchangeDate == currentDate)
                .OrderByDescending(e => e.ExchangeDate)
                .ToListAsync();
            foreach (var currency in currencyList)
            {
                var rate = exchangeRate.FirstOrDefault(e => e.ToCurrencyCode == currency.ToString());
                Assert.IsTrue(rate != null);
                Assert.IsTrue(rate.Rate > 0);
                Assert.IsTrue(rate.LastUpdatedDateUtc > DateTime.UtcNow.Date);
            }
        }

        [TestMethod]
        public async Task CalculateAllTodayExchangeRates()
        {
            var getAvailableCurrenciesResult = await Service.GetAvailableCurrencies();
            Assert.IsTrue(getAvailableCurrenciesResult.IsSuccess);
            var currencyList = getAvailableCurrenciesResult.Data.ToArray();

            var getAllRatesResult = await Service.GetAllExchangeRatesForToday();
            Assert.IsTrue(getAllRatesResult.IsSuccess);
            
            foreach (var fromCurrency in currencyList)
            {
                foreach (var toCurrency in currencyList)
                {
                    var rate = getAllRatesResult.Data
                        .FirstOrDefault(
                            x =>
                                x.FromCurrency == fromCurrency.Code
                                &&
                                x.ToCurrency == toCurrency.Code);
                    Assert.IsTrue(rate != null);
                    Assert.IsTrue(rate.ExchangeDate == DateTime.UtcNow.Date);
                    Assert.IsTrue(rate.FromCurrency == fromCurrency.Code);
                    Assert.IsTrue(rate.ToCurrency == toCurrency.Code);
                    Assert.IsTrue(rate.Rate > 0);
                    Assert.IsTrue(rate.MinDayRate > 0);
                    Assert.IsTrue(rate.MaxDayRate > 0);
                    Assert.IsTrue(rate.OpenDayRate > 0);
                    Assert.IsTrue(rate.MinDayRate <= rate.Rate);
                    Assert.IsTrue(rate.MaxDayRate >= rate.Rate);
                    Assert.IsTrue(rate.MinDayRate <= rate.OpenDayRate);
                    Assert.IsTrue(rate.MaxDayRate >= rate.OpenDayRate);
                }
            }
        }
    }
}
