using System;
using System.Collections.Generic;

namespace Infrastructure.DiagnosticTools
{
    public sealed class CodeExecutionMonitor : IDisposable
    {
        public interface ILogMeasuremnetWriter
        {
            void Write(CodeExecLogEntry logEntry);
        }
        [ThreadStatic]
        volatile static CodeExecutionMonitor current;
        ILogMeasuremnetWriter logWriter;
        public CodeExecutionMonitor(ILogMeasuremnetWriter logWriter, string blockName)
        {

            this.Block = blockName;
            this.Start = DateTime.Now;
            this._extraData = new Dictionary<string, string>();
            this.logWriter = logWriter;
            this.InnerCodeExecution = new List<CodeExecLogEntry>();
            parent = current;
            current = this;            
        }
        private CodeExecutionMonitor parent;
        public string Block { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
        public void AddExtraData(string key, string value)
        {
            if (_extraData.ContainsKey(key))
                _extraData[key] = value;

            else
            {
                _extraData.Add(key, value);
            }
        }

        public void Dispose()
        {
            End = DateTime.Now;
            var logEntry = CodeExecLogEntry.Create(this);            
            if (this.parent != null)
            {
                this.parent.InnerCodeExecution.Add(logEntry);
            }
            else
            {
                logEntry.AddChild(InnerCodeExecution);
                this.logWriter.Write(logEntry);
            }
            current = this.parent;
        }
       

        private Dictionary<string, string> _extraData;
        public IReadOnlyDictionary<string, string> ExtraData { get { return _extraData; } }
        List<CodeExecLogEntry> InnerCodeExecution { get; set; }

    }
}
