using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Scheduling.Service.Helpers.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using NLog;

namespace Scheduling.Service.Helpers
{
    public class HelperService : IHelperService
    {
        private static ILogger _logger;
        public HelperService()
        {
            SetUpNLog();
        }

        public async Task PerformTimerActivity(string schedule)
        {
            try
            {
                _logger.Info($"{DateTime.Now}: The PerformTimerActivity() is called with {schedule} schedule");
            }
            catch (Exception ex)
            {
                _logger.Error($"{DateTime.Now}: Exception is occured at PerformTimerActivity(): {ex.Message}");
                throw new CustomConfigurationException(ex.Message);
            }
        }

        private void SetUpNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "backupclientlogfile_helperservice.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

            _logger = LogManager.GetCurrentClassLogger();
        }
    }
}
