using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;
using Microsoft.EntityFrameworkCore;

namespace Avto.BL.Services.System.CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedAndNotifyIfNot
{
    public class CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedAndNotifyIfNotQueryHandler : QueryHandler<EmptyQuery, CallResult>
    {
        private SystemService SystemService { get; }
        private AppSettings Settings { get; }

        public CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedAndNotifyIfNotQueryHandler(
            Storage storage, 
            LogService logService,
            SystemService systemService,
            AppSettings settings) : base(storage, logService)
        {
            SystemService = systemService;
            Settings = settings;
        }

        protected override async Task<CallResult> HandleCommandAsync(EmptyQuery query)
        {
            if (Settings.Environment == "Development")
            {
                return SuccessResult();
            }

            if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday
                ||
                DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday
                ||
                DateTime.UtcNow.DayOfWeek == DayOfWeek.Monday)
            {
                LogService.WriteInfo("Skip exchange update at the weekend.");
                return SuccessResult();
            }
            
            var isAnyUpdateExists = await this.Storage.ExchangeRates
                .AnyAsync(e => e.LastUpdatedDateUtc >= DateTime.UtcNow.AddDays(-1));

            if (isAnyUpdateExists == false)
            {
                var mailMessage = $"CurrencyExchangeRates table is not updating.";
                await SystemService.SendSystemWarningEmail(mailMessage);
            }

            return SuccessResult();
        }
    }
}
