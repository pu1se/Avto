using System.Threading.Tasks;
using Avto.BL.Services.System;
using Microsoft.Extensions.DependencyInjection;

namespace Avto.Api.ScheduledJobs
{
    public class CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedScheduledJob : BaseScheduledJob<SystemService>
    {
        public CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedScheduledJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override async Task ExecuteScheduledJob(SystemService service)
        {
            await service.CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedAndNotifyIfNot();
        }
    }
}
