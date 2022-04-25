using System;
using System.Linq;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.ExchangeConfigs;
using Avto.BL.Services.Exchange.ExchangeProviders;
using Avto.DAL.Entities;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Exchange.CalculatedExchangeRates.Handlers.CalculateAllTodayExchangeRates
{
    public class CalculateAllTodayExchangeRatesCommandHandler : CommandHandler<EmptyCommand, CallResult>
    {
        private ExchangeProviderService ExchangeProviderService { get; }
        private ExchangeConfigService ExchangeConfigService { get; }
        private DateTime CurrentDate { get; }

        public CalculateAllTodayExchangeRatesCommandHandler(
            ExchangeProviderService exchangeProviderService,
            ExchangeConfigService exchangeConfigService,
            Storage storage, 
            LogService logger) : base(storage, logger)
        {
            ExchangeProviderService = exchangeProviderService;
            ExchangeConfigService = exchangeConfigService;
            CurrentDate = DateTime.UtcNow.Date;
        }

        protected override async Task<CallResult> HandleCommandAsync(EmptyCommand command)
        {
            await ExchangeProviderService.RefreshTodayRatesForAllProviders();
            await AddOrUpdateTodayCommonExchangeRates();
            await AddOrUpdateTodayExchangeRatesConfiguredByOrganizations();

            return SuccessResult();
        }

        private async Task AddOrUpdateTodayCommonExchangeRates()
        {
            Logger.WriteInfo($"{nameof(AddOrUpdateTodayCommonExchangeRates)}.");
            var getTodayCommonRatesResult = await ExchangeProviderService.GetTodayCommonExchangeRatesFromEcbWithSurcharge();
            if (!getTodayCommonRatesResult.IsSuccess)
            {
                Logger.WriteError($"Can not get common rates, because {getTodayCommonRatesResult.ErrorMessage}. " +
                                  $"Call result: {getTodayCommonRatesResult.ToJson()}");
                return;
            }

            var todayExistingExchangeRateEntities = await Storage.CalculatedCurrencyExchangeRates
                .Where(
                    e => 
                    e.ExchangeDate == CurrentDate
                    &&
                    e.OrganizationId == null)
                .ToListAsync();

            foreach (var newRate in getTodayCommonRatesResult.Data)
            {
                var existingExchangeRateEntity = todayExistingExchangeRateEntities
                    .FirstOrDefault(e => e.FromCurrency == newRate.FromCurrency && e.ToCurrency == newRate.ToCurrency);

                if (existingExchangeRateEntity == null)
                {
                    Storage.CalculatedCurrencyExchangeRates.Add(new CalculatedCurrencyExchangeRateEntity
                    {
                        FromCurrency = newRate.FromCurrency,
                        ToCurrency = newRate.ToCurrency,
                        OrganizationId = null,
                        ExchangeDate = newRate.ExchangeDate,
                        ExchangeRate = newRate.ExchangeRate
                    });
                }
                else
                {
                    existingExchangeRateEntity.ExchangeRate = newRate.ExchangeRate;
                }
            }

            await Storage.SaveChangesAsync();
        }

        private async Task AddOrUpdateTodayExchangeRatesConfiguredByOrganizations()
        {
            Logger.WriteInfo($"{nameof(AddOrUpdateTodayExchangeRatesConfiguredByOrganizations)}.");
            var organizationsExchangeConfigs = await Storage.CurrencyExchangeConfis.ToListAsync();

            foreach (var exchangeConfigs in organizationsExchangeConfigs.GroupBy(x => x.OrganizationId))
            {
                var organizationId = exchangeConfigs.Key;
                var getNewOrganizationRatesResult = await ExchangeConfigService.GetTodayRatesConfiguredByOrganization(organizationId);
                if (!getNewOrganizationRatesResult.IsSuccess)
                {
                    Logger.WriteError($"Can not get rates for organization {organizationId}. " +
                                           $"Error message: {getNewOrganizationRatesResult.ErrorMessage}. " +
                                           $"Response: {getNewOrganizationRatesResult.ToJson()}.");
                    continue;
                }

                var newOrganizationRates = getNewOrganizationRatesResult.Data;
                var existingTodayOrganizationRates = await Storage.CalculatedCurrencyExchangeRates
                    .Where(
                        e => 
                        e.ExchangeDate == CurrentDate 
                        && 
                        e.OrganizationId == organizationId)
                    .ToListAsync();
                foreach (var newRate in newOrganizationRates)
                {
                    var existingTodayRate = existingTodayOrganizationRates
                        .FirstOrDefault(
                            x => 
                            x.FromCurrency == newRate.FromCurrency 
                            && 
                            x.ToCurrency == newRate.ToCurrency);

                    if (existingTodayRate == null)
                    {
                        Storage.CalculatedCurrencyExchangeRates.Add(new CalculatedCurrencyExchangeRateEntity
                        {
                            FromCurrency = newRate.FromCurrency,
                            ToCurrency = newRate.ToCurrency,
                            OrganizationId = newRate.ProvidedByOrganizationId,
                            ExchangeDate = newRate.ExchangeDate,
                            ExchangeRate = newRate.ExchangeRateWithSurcharge
                        });
                    }
                    else
                    {
                        existingTodayRate.ExchangeRate = newRate.ExchangeRateWithSurcharge;
                    }
                }

                await Storage.SaveChangesAsync();
            }
        }
    }
}
