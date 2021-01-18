using System;
using System.Collections.Generic;
using System.Text;

namespace PublishYourIdea.Api.Application.Contracts.Services
{
    public interface ILoggerManagerService
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
