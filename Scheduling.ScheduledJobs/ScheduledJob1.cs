namespace Scheduling.ScheduledJobs
{
    using NLog;
    using Quartz;
    using System.Threading.Tasks;

    public class ScheduledJob1 : IJob
    {
        ILogger _logger;

        public ScheduledJob1(ILogger logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Info($"{nameof(ScheduledJob1)} has begun execution");
            return Task.CompletedTask;
        }
    }
}
