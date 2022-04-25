using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.BL.Services.Exchange.CalculatedExchangeRates;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.AddEditExchangeConfig;
using PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.DeleteExchangeConfig;
using PaymentMS.BL.Services.Exchange.ExchangeProviders;
using PaymentMS.DAL;
using PaymentMS.DAL.Enums;
using PaymentMS.Tests.BL.Base;
using Stripe;

namespace PaymentMS.Tests.BL
{
    [TestClass]
    public class ExchangeConfigServiceTests : BaseServiceTests<ExchangeConfigService>
    {
        private ExchangeProviderService ProviderService { get; set; }

        [TestInitialize]
        public void Init()
        {
            ProviderService = Resolve<ExchangeProviderService>();
            var providers = EnumHelper.ToList<ExchangeProviderType>();

            var currentDate = DateTime.UtcNow.Date;
            var exchangeRates = Storage.CurrencyExchangeRates
                .Where(e => e.ExchangeDate == currentDate)
                .ToListAsync().GetAwaiter().GetResult();

            foreach (var provider in providers)
            {
                if (!exchangeRates.Any(x => x.ExchangeProvider == provider))
                {
                    ProviderService.RefreshTodayRatesForProvider(provider).GetAwaiter().GetResult();
                }   
            }
        }

        [TestMethod]
        public async Task AddUpdateDeleteGetExchangeConfig()
        {
            var command = new AddEditExchangeConfigCommand
            {
                FromCurrency = CurrencyType.NZD,
                ToCurrency = CurrencyType.NOK,
                RateSourceProvider = ExchangeProviderType.ECB,
                OrganizationId = TestData.CurrencyOwnerOrganizationId,
                SurchargeAsPercent = 3,
            };
            var addResult = await Service.AddEditOrganizationExchangeConfig(command);

            Assert.IsTrue(addResult.IsSuccess);

            var getAddedConfigsResult = await Service.GetOrganizationExchangeConfigs(TestData.CurrencyOwnerOrganizationId);

            Assert.IsTrue(getAddedConfigsResult.IsSuccess);
            Assert.IsTrue(getAddedConfigsResult.Data.Any());
            var addedConfig = getAddedConfigsResult.Data.FirstOrDefault(
                x =>
                x.FromCurrency == command.FromCurrency &&
                x.ToCurrency == command.ToCurrency
            );
            Assert.IsTrue(addedConfig != null);
            Assert.IsTrue(addedConfig.SurchargeAsPercent == command.SurchargeAsPercent);



            command = new AddEditExchangeConfigCommand
            {
                FromCurrency = CurrencyType.NZD,
                ToCurrency = CurrencyType.NOK,
                RateSourceProvider = ExchangeProviderType.ECB,
                OrganizationId = TestData.CurrencyOwnerOrganizationId,
                SurchargeAsPercent = 4,
            };
            var updateResult = await Service.AddEditOrganizationExchangeConfig(command);

            Assert.IsTrue(updateResult.IsSuccess);

            var getUpdatedConfigsResult = await Service.GetOrganizationExchangeConfigs(TestData.CurrencyOwnerOrganizationId);

            Assert.IsTrue(getUpdatedConfigsResult.IsSuccess);
            Assert.IsTrue(getUpdatedConfigsResult.Data.Any());
            var updatedConfig = getUpdatedConfigsResult.Data.FirstOrDefault(
                x =>
                    x.FromCurrency == command.FromCurrency &&
                    x.ToCurrency == command.ToCurrency
            );
            Assert.IsTrue(updatedConfig != null);
            Assert.IsTrue(updatedConfig.SurchargeAsPercent == command.SurchargeAsPercent);

            var deleteCommand = new DeleteExchangeConfigCommand
            {
                FromCurrency = updatedConfig.FromCurrency,
                ToCurrency = updatedConfig.ToCurrency,
                OrganizationId = updatedConfig.OrganizationId
            };
            var deleteResult = await Service.DeleteOrganizationExchangeConfig(deleteCommand);
            Assert.IsTrue(deleteResult.IsSuccess);

            CleanStorageCache();
            var getConfigsResult = await Service.GetOrganizationExchangeConfigs(TestData.CurrencyOwnerOrganizationId);

            Assert.IsTrue(getConfigsResult.IsSuccess);
            var deletedConfig = getConfigsResult.Data.FirstOrDefault(
                x =>
                    x.FromCurrency == command.FromCurrency &&
                    x.ToCurrency == command.ToCurrency
            );
            Assert.IsTrue(deletedConfig == null);
        }

        [TestMethod]
        public async Task ValidationFail()
        {
            var getOrganizationRatesResult = await Service.GetOrganizationExchangeConfigs(new Guid());
            Assert.IsTrue(getOrganizationRatesResult.IsSuccess == false);
            Assert.IsTrue(getOrganizationRatesResult.ErrorType == ErrorType.ValidationError400);

            var addOrganizationConfigResult = await Service.AddEditOrganizationExchangeConfig(new AddEditExchangeConfigCommand());
            Assert.IsTrue(addOrganizationConfigResult.IsSuccess == false);
            Assert.IsTrue(addOrganizationConfigResult.ErrorType == ErrorType.ValidationError400);
        }

        [TestMethod]
        public async Task GetRates()
        {
            var getRateListResult = await Service.GetTodayRatesConfiguredByOrganization(TestData.CurrencyOwnerOrganizationId);
            Assert.IsTrue(getRateListResult.IsSuccess);

            await Resolve<CalculatedExchangeRatesService>().RefreshAndCalculateAllTodayExchangeRates();
            var getCalculatedRateListResult = await Resolve<CalculatedExchangeRatesService>()
                .GetCalculatedConfiguredByOrganizationExchangeRates(TestData.CurrencyOwnerOrganizationId, DateTime.UtcNow);
            Assert.IsTrue(getCalculatedRateListResult.IsSuccess);
            foreach (var organizationRate in getRateListResult.Data)
            {
                var calculatedRate = getCalculatedRateListResult.Data.FirstOrDefault(
                    x =>
                        x.FromCurrencyCode == organizationRate.FromCurrency.ToString()
                        &&
                        x.ToCurrencyCode == organizationRate.ToCurrency.ToString());

                Assert.IsNotNull(calculatedRate);
                Assert.IsTrue(calculatedRate.ExchangeRate == organizationRate.ExchangeRateWithSurcharge);
                Assert.IsTrue(calculatedRate.ExchangeDate == organizationRate.ExchangeDate);
                Assert.IsTrue(calculatedRate.ProvidedByOrganizationId == organizationRate.ProvidedByOrganizationId);
            }
        }
    }
}
