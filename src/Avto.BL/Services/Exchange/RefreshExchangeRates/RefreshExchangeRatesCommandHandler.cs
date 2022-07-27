using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.ExternalApis;
using Avto.BL.Services.Exchange.ExternalApis.ApiModels;
using Avto.DAL;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Avto.BL.Services.Exchange.RefreshExchangeRates
{
    public class RefreshExchangeRatesCommandHandler : CommandHandler<EmptyCommand, CallResult>
    {
        private CurrencyLayerApiProvider CurrencyLayerApi { get; }
        public RefreshExchangeRatesCommandHandler(
            Storage storage, 
            LogService logService,
            CurrencyLayerApiProvider currencyLayerApi) : base(storage, logService)
        {
            this.CurrencyLayerApi = currencyLayerApi;
        }

        protected override async Task<CallResult> HandleCommandAsync(EmptyCommand command)
        {
            try
            {
                if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday
                    ||
                    DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday)
                {
                    LogService.WriteInfo("Skip exchange update at the weekend.");
                    return SuccessResult();
                }

                LogService.WriteInfo("Refreshing rates.");
                var provider = CurrencyLayerApi;
                var getRateSourceResult = await SafeCallAsync(() => provider.GetLatestTodayExchangeRateListAsync());
                if (getRateSourceResult.IsSuccess)
                {
                    LogService.WriteInfo($"Save to table CurrencyExchangeRates rates from provider CurrencyLayer.");
                    await FillDataFromProvider(getRateSourceResult.Data);
                }
                else
                {
                    var errorMessage = $"Can not get rates from provider CurrencyLayer. " +
                                       $"Reason: {getRateSourceResult.ErrorMessage}. " +
                                       $"Call result {getRateSourceResult.ToJson()}";
                    LogService.WriteError(errorMessage);
                    return FailResult(errorMessage);
                }
            }
            catch (Exception exception)
            {
                LogService.WriteError($"Exception during refreshing rates from provider CurrencyLayer.");
                LogService.WriteError(exception);
                return FailResult(exception.Message);
            }

            return SuccessResult();
        }

        private async Task FillDataFromProvider(List<CurrencyLayerRateApiModel> rateList)
        {
            var currentDate = DateTime.UtcNow.Date;
            var exchangeRateList = await Storage.ExchangeRates
                .Where(
                    e => 
                        e.ExchangeDate == currentDate
                )
                .ToListAsync();

            foreach (var rate in rateList)
            {
                var rateToBeUpdated = exchangeRateList.FirstOrDefault(
                    e =>
                        e.FromCurrencyCode == rate.FromCurrency.ToString() 
                        &&
                        e.ToCurrencyCode == rate.ToCurrency.ToString()
                );
                if (rateToBeUpdated == null)
                {
                    var newExchangeRate = new ExchangeRateEntity
                    {
                        FromCurrencyCode = rate.FromCurrency.ToString(),
                        ToCurrencyCode = rate.ToCurrency.ToString(),
                        Rate = rate.Rate,
                        ExchangeDate = currentDate,
                        OpenDayRate = rate.Rate,
                        MinDayRate = rate.Rate,
                        MaxDayRate = rate.Rate,
                    };
                    Storage.ExchangeRates.Add(newExchangeRate);
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

                    Storage.ExchangeRates.Update(rateToBeUpdated);
                }
            }

            await Storage.SaveChangesAsync();
        }
    }
}
