using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PaymentMS.BL.Services.Exchange.CalculatedExchangeRates;

namespace PaymentMS.Api.ScheduledJobs
{
    public class CalculateAllTodayExchangeRatesScheduledJob : BaseScheduledJob<CalculatedExchangeRatesService>
    {
        public CalculateAllTodayExchangeRatesScheduledJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override Task ExecuteScheduledJob(CalculatedExchangeRatesService service)
        {
            return service.RefreshAndCalculateAllTodayExchangeRates();
        }
    }
}
