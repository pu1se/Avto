using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentMS.BL;
using PaymentMS.BL._Core.Logger;
using PaymentMS.BL.Services;
using PaymentMS.BL.Services.Exchange.ExchangeProviders;
using Quartz;

namespace PaymentMS.Api.ScheduledJobs
{
    public abstract class BaseScheduledJob<TService> : IJob where TService : BaseService
    {
        private IServiceScopeFactory ScopeFactory { get; }

        protected BaseScheduledJob(IServiceScopeFactory serviceScopeFactory)
        {
            ScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // todo: add logger initialization and saving.
            using (var scope = ScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var timer = new Stopwatch();

                timer.Start();
                await ExecuteScheduledJob(service);
                timer.Stop();
                
                var logger = scope.ServiceProvider.GetRequiredService<LogService>();
                await SaveLogs(logger, timer);
            }
        }

        public abstract Task ExecuteScheduledJob(TService service);

        private async Task SaveLogs(LogService logger, Stopwatch timer)
        {
            logger.AddHttpInfo(new LogHttpInfo
            {
                PathToAction = GetType().Name,
                ResponseCode = logger.HasErrors ? 500 : 200,
                HttpMethod = null,
                ExecutionTimeInMilliSec = timer.ElapsedMilliseconds
            });
            await logger.SaveAsync();
        }
    }
}
