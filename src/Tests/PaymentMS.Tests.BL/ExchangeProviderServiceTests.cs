using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.BL.Services.Exchange;
using PaymentMS.DAL;
using PaymentMS.DAL.Enums;
using PaymentMS.Tests.BL.Base;
using System.Linq;
using PaymentMS.BL;
using PaymentMS.BL._Core.Logger;
using PaymentMS.BL.Services.Exchange.CalculatedExchangeRates;
using PaymentMS.BL.Services.Exchange.Currency;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.AddEditExchangeConfig;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.DeleteExchangeConfig;
using PaymentMS.BL.Services.Exchange.ExchangeProviders;
using PaymentMS.BL.Services.Exchange.Handlers.Queries.GetProviderExchangeRates;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class ExchangeProviderServiceTests : BaseServiceTests<ExchangeProviderService>
    {
        [TestMethod]
        public async Task GetExchangeProviders()
        {
            var getProvidersResult = await Service.GetExchangeProviders();

            Assert.IsTrue(getProvidersResult.IsSuccess);
            Assert.IsTrue(getProvidersResult.Data.Any());
            foreach (var item in getProvidersResult.Data)
            {
                Assert.IsTrue(!item.Id.IsNullOrEmpty());
                Assert.IsTrue(!item.Name.IsNullOrEmpty());
            }
        }

        [TestMethod]
        public async Task RefreshTodayRatesForAllProvider()
        {
            var refreshRatesResult = await Service.RefreshTodayRatesForAllProviders();
            Assert.IsTrue(refreshRatesResult.IsSuccess);
        }

        [TestMethod]
        public async Task GetFiltredExchangeProviders()
        {
            var getFiltredProvidersResult = await Service.GetExchangeProviders(CurrencyType.IDR);

            Assert.IsTrue(getFiltredProvidersResult.IsSuccess);
            Assert.IsTrue(getFiltredProvidersResult.Data.Any());
            foreach (var item in getFiltredProvidersResult.Data)
            {
                Assert.IsTrue(!item.Id.IsNullOrEmpty());
                Assert.IsTrue(!item.Name.IsNullOrEmpty());
            }
        }

        [TestMethod]
        public async Task CheckThatSupportedCurrenciesAreEquelToCurrencyEnum()
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
        public async Task RefreshAndGetRateFromAllProviderExceptCustomer()
        {
            var getProvidersResult = await Service.GetExchangeProviders();
            var testingProviders = getProvidersResult.Data
                .Where(x => x.Id.IsCustomProvider() == false)
                .Select(x => x.Id.AsEnum<ExchangeProviderType>());

            var currencyService = Resolve<CurrencyService>();
            var getCurrencyListResult = await currencyService.GetAvailableCurrencies();
            var currencyList = getCurrencyListResult.Data
                .Select(x => x.Code.AsEnum<CurrencyType>())
                .ToList();
                

            foreach (var provider in testingProviders)
            {

                var refreshRatesResult = await Service.RefreshTodayRatesForProvider(provider);
                Assert.IsTrue(refreshRatesResult.IsSuccess);

                foreach (var fromCurrency in currencyList)
                {
                    foreach (var toCurrency in currencyList)
                    {
                        
                        var getExchangeQuery = new GetTodayRateFromProviderQuery
                        {
                            Provider = provider,
                            FromCurrency = fromCurrency,
                            ToCurrency = toCurrency,
                            OrganizationId = TestData.CurrencyOwnerOrganizationId
                        };


                        var getRateResult = await Service.GetTodayRateFromProvider(getExchangeQuery);
                        Assert.IsTrue(getRateResult.IsSuccess);
                        var rate = getRateResult.Data;

                        Assert.IsTrue(rate != null);
                        Assert.IsTrue(rate.ExchangeRate > 0);
                        Assert.IsTrue(rate.ExchangeDate >= DateTime.UtcNow.AddDays(-1));   
                        Assert.IsTrue(rate.ExchangeDate <= DateTime.UtcNow); 
                    }
                }

            }
            
        }

        [TestMethod]
        public async Task RefreshCustomManualProvider()
        {
            var provider = ExchangeProviderType.Custom;

            var configService = Resolve<ExchangeConfigService>();

            var addCustomerProviderCommand = new AddEditExchangeConfigCommand
            {
                FromCurrency = CurrencyType.EUR,
                ToCurrency = CurrencyType.BRL,
                OrganizationId = TestData.CurrencyOwnerOrganizationId,
                CustomRate = 12,
                RateSourceProvider = ExchangeProviderType.Custom,
                SurchargeAsPercent = 5
            };
            await configService.AddEditOrganizationExchangeConfig(addCustomerProviderCommand);
                
            var refreshRatesResult = await Service.RefreshTodayRatesForProvider(provider);
            Assert.IsTrue(refreshRatesResult.IsSuccess);

            var getExchangeQuery = new GetTodayRateFromProviderQuery
            {
                Provider = addCustomerProviderCommand.RateSourceProvider,
                FromCurrency = addCustomerProviderCommand.FromCurrency,
                ToCurrency = addCustomerProviderCommand.ToCurrency,
                OrganizationId = addCustomerProviderCommand.OrganizationId
            };


            var getRateFromProviderResult = await Service.GetTodayRateFromProvider(getExchangeQuery);
            Assert.IsTrue(getRateFromProviderResult.IsSuccess);
            var rateFromProvider = getRateFromProviderResult.Data;

            Assert.IsTrue(rateFromProvider != null);
            Assert.IsTrue(rateFromProvider.ExchangeRate > 0);
            Assert.IsTrue(rateFromProvider.ExchangeDate >= DateTime.UtcNow.AddDays(-1));   
            Assert.IsTrue(rateFromProvider.ExchangeDate <= DateTime.UtcNow); 
            Assert.IsTrue(rateFromProvider.ExchangeProvider == addCustomerProviderCommand.RateSourceProvider); 
            Assert.IsTrue(rateFromProvider.ExchangeRate == addCustomerProviderCommand.CustomRate);


            var exchangeConfigService = Resolve<ExchangeConfigService>();
            var getRateResult = await exchangeConfigService.GetTodayRatesConfiguredByOrganization(addCustomerProviderCommand.OrganizationId);
            Assert.IsTrue(getRateResult.IsSuccess);
            var rate = getRateResult.Data
                .FirstOrDefault(
                    x => 
                    x.FromCurrency == addCustomerProviderCommand.FromCurrency 
                    &&
                    x.ToCurrency == addCustomerProviderCommand.ToCurrency);
            Assert.IsTrue(rate != null);
            Assert.IsTrue(rate.ExchangeRate > 0);
            Assert.IsTrue(rate.ExchangeDate >= DateTime.UtcNow.AddDays(-1));   
            Assert.IsTrue(rate.ExchangeDate <= DateTime.UtcNow); 
            Assert.IsTrue(rate.ExchangeRate == addCustomerProviderCommand.CustomRate);
            var calculatedRateWithSurcharge = addCustomerProviderCommand.CustomRate * (1 + addCustomerProviderCommand.SurchargeAsPercent / 100);
            Assert.IsTrue(rate.ExchangeRateWithSurcharge == calculatedRateWithSurcharge);


            await configService.DeleteOrganizationExchangeConfig(new DeleteExchangeConfigCommand
            {
                OrganizationId = addCustomerProviderCommand.OrganizationId,
                FromCurrency = addCustomerProviderCommand.FromCurrency,
                ToCurrency = addCustomerProviderCommand.ToCurrency
            });
        }
    }
}
