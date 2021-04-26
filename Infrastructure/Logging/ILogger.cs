using System;

namespace Infrastructure.Logging
{
    /// <summary>
    /// Contract interface for various logging methods that should be available to the system
    /// </summary>
    public interface ILogger
    {
        void LogError(string message);

        void LogError(string message, Exception exception);

        void LogInformation(string message);

        void LogInformation(string message, Exception exception);

        void LogInWarning(string message);

        void LogInWarning(string message, Exception exception);

        void LogInDebug(string message);

        void LogInDebug(string message, Exception exception);
    }
}