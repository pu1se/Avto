using System.Threading.Tasks;
using Avto.BL.Services.ParseWebPage;
using Microsoft.Extensions.DependencyInjection;

namespace Avto.Api.ScheduledJobs
{
    public class ParseWebPagesSheduledJob : BaseScheduledJob<ParseWebPageService>
    {
        public ParseWebPagesSheduledJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override Task ExecuteScheduledJob(ParseWebPageService service)
        {
            return service.ParseBelavia();
        }
    }
}
