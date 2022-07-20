using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Avto.BL;
using Avto.BL._Core.Logger;
using Avto.BL.Services;
using Avto.DAL;
using Quartz;

namespace Avto.Api.ScheduledJobs
{
    public abstract class BaseScheduledJob<TService> : IJob where TService : BaseService
    {
        private IServiceScopeFactory ScopeFactory { get; }
        private string ScheduledJobName { get; }

        protected BaseScheduledJob(IServiceScopeFactory serviceScopeFactory)
        {
            ScopeFactory = serviceScopeFactory;
            ScheduledJobName = GetType().Name;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<LogService>();
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var timer = new Stopwatch();


                timer.Start();
                try
                {
                    await ExecuteScheduledJob(service);
                }
                catch (Exception ex)
                {
                    logger.WriteError(ex);
                }
                finally
                {
                    timer.Stop();
                    await SaveLogs(logger, timer);
                }
            }
        }

        public abstract Task ExecuteScheduledJob(TService service);

        private async Task SaveLogs(LogService logger, Stopwatch timer)
        {
            logger.AddHttpInfo(new LogHttpInfo
            {
                PathToAction = ScheduledJobName,
                ResponseCode = logger.HasErrors ? 500 : 200,
                HttpMethod = null,
                ExecutionTimeInMilliSec = timer.ElapsedMilliseconds
            });
            await logger.SaveAsync();
        }
    }
}
