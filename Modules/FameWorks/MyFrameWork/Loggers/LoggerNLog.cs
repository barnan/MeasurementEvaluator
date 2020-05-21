using MyFrameWork.Interfaces;
using NLog;

namespace MyFrameWork.Loggers
{
    internal class LoggerNLog : IMyLogger
    {
        private readonly ILogger _logger;
        public LoggerNLog(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public string Trace(string message)
        {
            _logger.Trace(message);
            return message;
        }

        public string Debug(string message)
        {
            _logger.Trace(message);
            return message;
        }

        public string Info(string message)
        {
            _logger.Trace(message);
            return message;
        }

        public string Warning(string message)
        {
            _logger.Trace(message);
            return message;
        }

        public string Error(string message)
        {
            _logger.Trace(message);
            return message;
        }

        public string Fatal(string message)
        {
            _logger.Trace(message);
            return message;
        }

        public bool IsTraceEnabled() => _logger.IsTraceEnabled;

        public bool IsDebugEnabled() => _logger.IsDebugEnabled;

        public bool IsInfoEnabled() => _logger.IsInfoEnabled;

        public bool IsWarningEnabled() => _logger.IsWarnEnabled;

        public bool IsErrorEnabled() => _logger.IsErrorEnabled;

        public bool IsFatalEnabled() => _logger.IsFatalEnabled;
    }
}
