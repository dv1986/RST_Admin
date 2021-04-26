
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DiagnosticTools;

namespace RST.Admin.Web.Api.Helper
{
    public sealed class DefaultLogMeasuremnetWriter : CodeExecutionMonitor.ILogMeasuremnetWriter
    {
        Logger logger;
        public DefaultLogMeasuremnetWriter(bool enabled, string logpath)
        {
            if (enabled && !string.IsNullOrWhiteSpace(logpath))
            {
                logger = new LoggerConfiguration().WriteTo.RollingFile(logpath).CreateLogger();
            }
        }

        public void Write(CodeExecLogEntry logEntry)
        {
            if (logger != null)
            {
                logger.Information(logEntry.ToString());
            }
        }
    }
}
