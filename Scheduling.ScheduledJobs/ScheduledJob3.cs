namespace Scheduling.ScheduledJobs
{
    using NLog;
    using Quartz;
    using System.Threading.Tasks;

    public class ScheduledJob3 : IJob
    {
        ILogger _logger;

        public ScheduledJob3(ILogger logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"{nameof(ScheduledJob3)} has begun execution");
            return Task.CompletedTask;
        }
    }
}
