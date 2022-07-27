using System.Threading.Tasks;
using Avto.BL.Services.System;
using Microsoft.Extensions.DependencyInjection;

namespace Avto.Api.ScheduledJobs
{
    public class CheckLogsForTodayErrorsAndNotifyAboutThemScheduledJob : BaseScheduledJob<SystemService>
    {
        public CheckLogsForTodayErrorsAndNotifyAboutThemScheduledJob(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override Task ExecuteScheduledJob(SystemService service)
        {
            return service.CheckLogsForTodayErrorsAndNotifyAboutThem();
        }
    }
}
