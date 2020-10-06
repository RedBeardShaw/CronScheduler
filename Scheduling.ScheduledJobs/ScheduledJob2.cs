namespace Scheduling.ScheduledJobs
{
    using NLog;
    using Quartz;
    using Scheduling.Contracts;
    using System.Threading.Tasks;

    public class ScheduledJob2 : IScheduledJob
    {
        ILogger _logger;

        public ScheduledJob2(ILogger logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"{nameof(ScheduledJob2)} has begun execution");
            return Task.CompletedTask;
        }
    }
}
