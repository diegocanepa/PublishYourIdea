using NLog;
using PublishYourIdea.Api.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Application.Services
{
    public class LoggerManagerService : ILoggerManagerService
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManagerService()
        {
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
        public void LogError(string message)
        {
            logger.Error(message);
        }
        public void LogInfo(string message)
        {
            logger.Info(message);
        }
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
