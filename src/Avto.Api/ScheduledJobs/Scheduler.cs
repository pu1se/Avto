using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Avto.Api.ScheduledJobs;
using Quartz;
using Quartz.Impl;

namespace Avto.Api.JobsScheduler
{
    public static class Scheduler
    {
        public static void StartAllScheduledJobs(IServiceCollection services)
        {
            var scheduler = CreateScheduler(services);

            scheduler.StartJobFor<RefreshExchangeRatesScheduledJob>(withIntervalInHours: 1);
            scheduler.StartJobFor<CheckLogsForTodayErrorsAndNotifyAboutThemScheduledJob>(withIntervalInHours: 24);
            scheduler.StartJobFor<CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedScheduledJob>(withIntervalInHours: 24);
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
            // maybe make JobFactory a singleton 
            services.AddTransient<JobFactory>();

            var allScheduledJobs = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.BaseType.Name == typeof(BaseScheduledJob<>).Name).ToList();
            foreach (var type in allScheduledJobs)
            {
                services.AddScoped(type);
            }
        }

        private static void StartJobFor<TScheduledJob>(
            this IScheduler scheduler, 
            int withIntervalInMinutes = 0,
            int withIntervalInHours = 0) where TScheduledJob : IJob
        {
            if (withIntervalInMinutes == 0 && withIntervalInHours == 0)
            {
                throw new ArgumentException("Either withIntervalInMinutes or withIntervalInHours must be greater than 0");
            }

            if (withIntervalInHours > 0)
            {
                withIntervalInMinutes += withIntervalInHours * 60;
            }

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
