using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Avto.BL.Services.Exchange.ExchangeProviders.API;
using Avto.DAL.Enums;
using Avto.Tests.BL.Base;

namespace Avto.Tests.BL
{
    [TestClass]
    public class EcbApiTests : BaseServiceTests<EcbProviderApi>
    {
        [TestMethod]
        public async Task SuccessEcbXmlParsing()
        {
            var exchangeRateList = await Service.GetLatestTodayExchangeRateListAsync();

            foreach (var currency in Service.GetSupportedCurrencies())
            {
                var rateForOneCurrency = exchangeRateList.Where(x => x.ToCurrency == currency);
                foreach (var rate in rateForOneCurrency)
                {
                    Assert.IsTrue(rate.Rate > 0);
                }
            }
        }
    }
}
