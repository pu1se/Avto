using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.Api.ApiModels;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.API;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.GetExchangeRatesFromEcbWithSurcharge;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.RefreshAllProvidersTodayRates;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Commands.RefreshAllProvidersTodayRates
{
    public class RefreshTodayRatesForProviderCommandHandler : CommandHandler<RefreshTodayRatesForProviderCommand, CallResult>
    {
        private ProviderFactory ProviderFactory { get; }

        public RefreshTodayRatesForProviderCommandHandler(
            ProviderFactory providerFactory,
            Storage storage,
            LogService logger) : base(storage, logger)
        {
            ProviderFactory = providerFactory;
        }

        protected override async Task<CallResult> HandleCommandAsync(RefreshTodayRatesForProviderCommand command)
        {
            Logger.WriteInfo("Fill available currencies.");
            await FillAvailableCurrencies();

            var providers = ProviderFactory.GetSupportedProviders();
            if (command.RefreshForProvider.HasValue)
            {
                Logger.WriteInfo($"Refresh rates for provider {command.RefreshForProvider.Value}.");
                providers = providers.Where(x => x == command.RefreshForProvider.Value).ToList();
            }
            else
            {
                Logger.WriteInfo($"Refresh rates for all providers.");
            }

            foreach (var providerType in providers)
            {
                try
                {
                    var provider = ProviderFactory.GetProvider(providerType);

                    var getRateSourceResult = await SafeCallAsync(() => provider.GetLatestTodayExchangeRateListAsync());
                    if (getRateSourceResult.IsSuccess)
                    {
                        Logger.WriteInfo($"Save to table CurrencyExchangeRates rates from provider {providerType}.");
                        await FillDataFromProvider(providerType, getRateSourceResult.Data);
                    }
                    else
                    {
                        Logger.WriteError($"Can not get rates from provider {providerType}. " +
                                          $"Reason: {getRateSourceResult.ErrorMessage}. " +
                                          $"Call result {getRateSourceResult.ToJson()}");
                    }
                }
                catch (Exception exception)
                {
                    Logger.WriteError($"Exception during refreshing rates from provider {providerType}.");
                    Logger.WriteError(exception);
                }
            }

            return SuccessResult();
        }

        private async Task FillDataFromProvider(ExchangeProviderType providerType, List<RateApiModel> rateList)
        {
            var currentDate = DateTime.UtcNow.Date;
            var exchangeRateList = await Storage.CurrencyExchangeRates
                .Where(
                    e => 
                        e.ExchangeProvider == providerType &&
                        e.ExchangeDate == currentDate
                )
                .ToListAsync();

            foreach (var rate in rateList)
            {
                var rateToBeUpdated = exchangeRateList.FirstOrDefault(
                    e =>
                        e.FromCurrencyCode == rate.FromCurrency.ToString() &&
                        e.ToCurrencyCode == rate.ToCurrency.ToString() &&
                        e.OrganizationId == rate.OrganizationId
                );
                if (rateToBeUpdated == null)
                {
                    var newExchangeRate = new CurrencyExchangeRateEntity
                    {
                        FromCurrencyCode = rate.FromCurrency.ToString(),
                        ToCurrencyCode = rate.ToCurrency.ToString(),
                        Rate = rate.Rate,
                        ExchangeDate = currentDate,
                        ExchangeProvider = providerType,
                        OrganizationId = rate.OrganizationId,
                        OpenDayRate = rate.Rate,
                        MinDayRate = rate.Rate,
                        MaxDayRate = rate.Rate,
                        HasExtraInfoForRate = true,
                    };
                    Storage.CurrencyExchangeRates.Add(newExchangeRate);
                }
                else
                {
                    rateToBeUpdated.Rate = rate.Rate;

                    if (rateToBeUpdated.MinDayRate > rate.Rate)
                    {
                        rateToBeUpdated.MinDayRate = rate.Rate;
                    }

                    if (rateToBeUpdated.MaxDayRate < rate.Rate)
                    {
                        rateToBeUpdated.MaxDayRate = rate.Rate;
                    }

                    Storage.CurrencyExchangeRates.Update(rateToBeUpdated);
                }
            }

            await Storage.SaveChangesAsync();
        }

        private async Task FillAvailableCurrencies()
        {
            foreach (var currencyItem in EnumHelper.ToList<CurrencyType>())
            {
                var isExists = await Storage.Currencies
                    .ExistsAsync(e => e.Code == currencyItem.ToString());

                if (!isExists)
                {
                    Storage.Currencies.Add(new CurrencyEntity
                    {
                        Code = currencyItem.ToString(),
                        Name = EnumHelper.GetDescription(currencyItem),
                    });
                }
            }

            await Storage.SaveChangesAsync();
        }

    }
}
