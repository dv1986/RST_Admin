using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DiagnosticTools
{
    [Serializable]
    public sealed class CodeExecLogEntry
    {
        private CodeExecLogEntry()
        {
            innerCodeExecution = new List<CodeExecLogEntry>();
        }

        public static CodeExecLogEntry Create(CodeExecutionMonitor monitor)
        {
            var logEntry = new CodeExecLogEntry()
            {
                Block = monitor.Block,
                Start = monitor.Start,
                End = monitor.End,
                ExtraData = monitor.ExtraData
            };
            return logEntry;
        }
        static CodeExecLogEntry()
        {
            _processId = Process.GetCurrentProcess().Id;
            
        }
        private static int _processId;
        public string Block { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public int ProcessId { get { return _processId; } }
        public double Duration { get { return (End - Start).TotalMilliseconds; } }
        
        public IReadOnlyDictionary<string, string> ExtraData { get; private set; }
        public override string ToString()
        {
            char colon = ',';
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine().Append("{");
            stringBuilder.AppendLine($"\"{nameof(ProcessId) }\" : {ProcessId}{colon}");
            stringBuilder.AppendLine($"\"{nameof(Block) }\": \"{Block}\"{colon}");
            stringBuilder.AppendLine($"\"{nameof(Start) }\" : \"{Start.ToString("hh:mm:ss:ms")}\"{colon}");
            stringBuilder.AppendLine($"\"{nameof(End) }\": \"{End.ToString("hh:mm:ss:ms")}\"{colon}");
            stringBuilder.AppendLine($"\"{nameof(Duration) }\": {Duration}");
            stringBuilder.AppendLine($"\"ThreadId\": {Thread.CurrentThread.ManagedThreadId}");
            if (ExtraData.Any())
            {
                var entries = ExtraData.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
                stringBuilder.AppendLine($"{colon}\"{nameof(ExtraData) }\" : {"{ " + string.Join(",", entries) + "}"}");
            }
            foreach (var item in innerCodeExecution)
            {
                stringBuilder.AppendLine($"{colon}\"{nameof(innerCodeExecution) }\" : {"{ " + item.ToString() + "}"}");
            }             
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }
        List<CodeExecLogEntry> innerCodeExecution;
        internal void AddChild(List<CodeExecLogEntry> children)
        {
            innerCodeExecution.AddRange(children);
        }
    }
}
