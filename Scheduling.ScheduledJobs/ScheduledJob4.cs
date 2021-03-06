﻿namespace Scheduling.ScheduledJobs
{
    using NLog;
    using Quartz;
    using System.Threading.Tasks;

    public class ScheduledJob4 : IJob
    {
        ILogger _logger;

        public ScheduledJob4(ILogger logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"{nameof(ScheduledJob4)} has begun execution");
            return Task.CompletedTask;
        }
    }
}
