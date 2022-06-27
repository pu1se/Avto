using System.Threading.Tasks;
using Avto.BL.Services.Exchange;
using Microsoft.Extensions.DependencyInjection;

namespace Avto.Api.ScheduledJobs
{
    public class RefreshExchangeRatesScheduledJob : BaseScheduledJob<ExchangeService>
    {
        public RefreshExchangeRatesScheduledJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override async Task ExecuteScheduledJob(ExchangeService service)
        {
            await service.RefreshExchangeRates();
        }
    }
}
