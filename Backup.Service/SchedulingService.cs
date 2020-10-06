using Scheduling.Service.Helpers;
using Scheduling.Service.Helpers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Configuration;
using Scheduling.ScheduledJobs;
using Scheduling.Contracts;

namespace Scheduling.Service
{
    public class SchedulingService : IHostedService
    {
        private static ILogger _logger;
        public SchedulingService()
        {
            SetUpNLog();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var scheduler = await GetScheduler();
                var serviceProvider = GetConfiguredServiceProvider();
                scheduler.JobFactory = new CustomJobFactory(serviceProvider);
                await ConfigureJob(scheduler, "Job1", GetJobDetail<ScheduledJob1>());
                await ConfigureJob(scheduler, "Job2", GetJobDetail<ScheduledJob2>());
                await ConfigureJob(scheduler, "Job3", GetJobDetail<ScheduledJob3>());
                await ConfigureJob(scheduler, "Job4", GetJobDetail<ScheduledJob4>());
                await scheduler.Start();
            }
            catch (Exception ex)
            {
                _logger.Error(new CustomConfigurationException(ex.Message));
            }
        }

        private async Task ConfigureJob(IScheduler scheduler, string jobName, IJobDetail jobDetail)
        {
            if (await scheduler.CheckExists(jobDetail.Key))
            {
                await scheduler.ResumeJob(jobDetail.Key);
                _logger.Info($"The job key {jobDetail.Key} was already existed, thus resuming the same");
            }
            else
            {
                ITrigger trigger = GetJobTrigger(jobName);
                await scheduler.ScheduleJob(jobDetail, trigger);
                _logger.Info($"The job key {jobDetail.Key} is schedulded to run next at {trigger.GetNextFireTimeUtc()}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #region "Private Functions"
        private IServiceProvider GetConfiguredServiceProvider()
        {
            var services = new ServiceCollection()
                .AddSingleton(_logger)
                .AddScoped<IScheduledJob, ScheduledJob1>()
                .AddScoped<IScheduledJob, ScheduledJob2>()
                .AddScoped<IScheduledJob, ScheduledJob3>()
                .AddScoped<IScheduledJob, ScheduledJob4>()
                .AddScoped<IHelperService, HelperService>();
            return services.BuildServiceProvider();
        }

        private IJobDetail GetJobDetail<T>() where T : IScheduledJob
        {
            IJobDetail jobDetail = JobBuilder.Create<T>()
                .WithIdentity($"DetailFor{typeof(T)}")
                .Build();
            return jobDetail;
        }

        private ITrigger GetJobTrigger(string jobName)
        {
            string configValue = ConfigurationManager.AppSettings[$"ScheduleFor{jobName}"];
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"TriggerFor{jobName}")
                .WithCronSchedule(configValue)
                .Build();
            return trigger;
        }

        private static async Task<IScheduler> GetScheduler()
        {
            // Uncomment this region if you want to use database
            #region
            //var config = (NameValueCollection)ConfigurationManager.GetSection("quartz");
            //var factory = new StdSchedulerFactory(config);
            #endregion

            // Uncomment this if you want to use RAM instead of database
            #region
            var props = new NameValueCollection { { "quartz.serializer.type", "binary" } };
            var factory = new StdSchedulerFactory(props);
            #endregion
            var scheduler = await factory.GetScheduler();
            return scheduler;
        }
        private void SetUpNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "backupclientlogfile_backupservice.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
            // Apply config           
            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }
        #endregion
    }
}
