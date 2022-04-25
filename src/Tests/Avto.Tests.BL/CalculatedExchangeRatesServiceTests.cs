using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Avto.BL.Services.Exchange.CalculatedExchangeRates;
using Avto.BL.Services.Exchange.Currency;
using Avto.BL.Services.Exchange.ExchangeProviders;
using Avto.Tests.BL.Base;

namespace Avto.Tests.BL
{
    [TestClass]
    public class CalculatedExchangeRatesServiceTests : BaseServiceTests<CalculatedExchangeRatesService>
    {
        [TestMethod]
        public async Task CalculateAllTodayExchangeRates()
        {
            var refreshRatesResult = await Service.RefreshAndCalculateAllTodayExchangeRates();
            Assert.IsTrue(refreshRatesResult.IsSuccess);

            var getCommonRatesResult = await Service.GetCalculatedCommonExchangeRates(DateTime.UtcNow);
            Assert.IsTrue(getCommonRatesResult.IsSuccess);

            var getAvailableCurrenciesResult = await this.Resolve<CurrencyService>().GetAvailableCurrencies();
            Assert.IsTrue(getAvailableCurrenciesResult.IsSuccess);

            var currencyList = getAvailableCurrenciesResult.Data.ToArray();
            foreach (var fromCurrency in currencyList)
            {
                foreach (var toCurrency in currencyList)
                {
                    var commonRate = getCommonRatesResult.Data
                        .FirstOrDefault(
                            x =>
                                x.FromCurrencyCode == fromCurrency.Code
                                &&
                                x.ToCurrencyCode == toCurrency.Code);
                    Assert.IsTrue(commonRate != null);
                }
            }

            var getCommonRateListFromEcbWithSurchargeResult = await Resolve<ExchangeProviderService>()
                .GetTodayCommonExchangeRatesFromEcbWithSurcharge();
            Assert.IsTrue(getCommonRateListFromEcbWithSurchargeResult.IsSuccess);
            foreach (var ecbRate in getCommonRateListFromEcbWithSurchargeResult.Data)
            {
                var calculatedRate = getCommonRatesResult.Data.FirstOrDefault(
                    x =>
                        x.FromCurrencyCode == ecbRate.FromCurrency.ToString()
                        &&
                        x.ToCurrencyCode == ecbRate.ToCurrency.ToString());

                Assert.IsNotNull(calculatedRate);
                Assert.IsTrue(calculatedRate.ExchangeRate == ecbRate.ExchangeRate);
                Assert.IsTrue(calculatedRate.ExchangeDate == ecbRate.ExchangeDate);
            }
        }
    }
}
