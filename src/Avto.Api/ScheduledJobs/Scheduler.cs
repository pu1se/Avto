using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Avto.Api.ScheduledJobs;
using Avto.BL.Services;
using Avto.BL.Services.Exchange.ExchangeProviders;
using Quartz;
using Quartz.Impl;

namespace Avto.Api.JobsScheduler
{
    public static class Scheduler
    {
        public static void StartAllScheduledJobs(IServiceCollection services)
        {
            var scheduler = CreateScheduler(services);

            scheduler.StartJobFor<CalculateAllTodayExchangeRatesScheduledJob>(withIntervalInMinutes: 60);
        }


        #region private methods
        private static IScheduler CreateScheduler(IServiceCollection services)
        {
            services.AddScheduledJobsAsScoped();
            var scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            scheduler.JobFactory = services.BuildServiceProvider().GetRequiredService<JobFactory>();
            scheduler.Start().GetAwaiter().GetResult();
            return scheduler;
        }

        private static void AddScheduledJobsAsScoped(this IServiceCollection services)
        {
            services.AddTransient<JobFactory>();

            var allScheduledJobs = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.BaseType.Name == typeof(BaseScheduledJob<>).Name).ToList();
            foreach (var type in allScheduledJobs)
            {
                services.AddScoped(type);
            }
        }

        private static void StartJobFor<TScheduledJob>(this IScheduler scheduler, int withIntervalInMinutes) where TScheduledJob : IJob
        {
            var job = JobBuilder.Create<TScheduledJob>().Build();

            var trigger = TriggerBuilder.Create()  
                .WithIdentity("Trigger_" + typeof(TScheduledJob).Name)
                .StartNow()                            
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(withIntervalInMinutes)
                    .RepeatForever())                   
                .Build();                              
 
            scheduler.ScheduleJob(job, trigger).GetAwaiter().GetResult();
        }
        #endregion private methods
    }
}
