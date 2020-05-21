using MyFrameWork.Interfaces;
using System.Diagnostics;

namespace MyFrameWork.Loggers
{
    internal class LoggerToTrace : IMyLogger
    {
        private readonly string _outputFileName = "TraceLogger.log";
        private static TraceSwitch _switch = new TraceSwitch("MyAssemblyTraceSwitch", "Application Tracing");


        public LoggerToTrace()
        {
            //var valami = System.Configuration.ConfigurationManager.AppSettings;
        }


        public string Trace(string message)
        {
            System.Diagnostics.Trace.WriteLineIf(_switch.TraceVerbose, message);
            return message;
        }

        public string Debug(string message)
        {
            System.Diagnostics.Trace.WriteLineIf(_switch.TraceVerbose, message);
            return message;
        }

        public string Info(string message)
        {
            System.Diagnostics.Trace.WriteLineIf(_switch.TraceInfo, message);
            return message;
        }

        public string Warning(string message)
        {
            System.Diagnostics.Trace.WriteLineIf(_switch.TraceWarning, message);
            return message;
        }

        public string Error(string message)
        {
            System.Diagnostics.Trace.WriteLineIf(_switch.TraceError, message);
            return message;
        }

        public string Fatal(string message)
        {
            System.Diagnostics.Trace.WriteLineIf(_switch.TraceError, message);
            return message;
        }

        public bool IsTraceEnabled() => _switch.TraceVerbose;

        public bool IsDebugEnabled() => _switch.TraceVerbose;

        public bool IsInfoEnabled() => _switch.TraceInfo;

        public bool IsWarningEnabled() => _switch.TraceWarning;

        public bool IsErrorEnabled() => _switch.TraceError;

        public bool IsFatalEnabled() => _switch.TraceError;
    }
}
