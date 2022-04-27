using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.ExternalApis;
using Avto.Tests.BL.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Avto.Tests.BL
{
    [TestClass]
    public class CurrencyLayerApiTests : BaseServiceTests<CurrencyLayerApiProvider>
    {
        [TestMethod]
        public async Task SuccessCurrencyLayerJsonParsing()
        {
            var exchangeRateFreshResult = await Service.GetLatestTodayExchangeRateListAsync();

            foreach (var currency in Service.GetSupportedCurrencies())
            {
                var rateForOneCurrency = exchangeRateFreshResult.Where(x => x.ToCurrency == currency);
                foreach (var rate in rateForOneCurrency)
                {
                    Assert.IsTrue(rate.Rate > 0);
                }
            }
        }
    }
}
