using FrameInterfaces;
using NLog;

namespace StandardLogger
{
    public class StandardLogger : IMyLogger
    {
        private static Dictionary<string, IMyLogger> _loggers;
        private ILogger _logger;
        
        static StandardLogger()
        {
            _loggers = new Dictionary<string, IMyLogger>();
        }

        public StandardLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static IMyLogger GetLogger(string name)
        {
            if (_loggers.ContainsKey(name))
            {
                return _loggers[name];
            }

            _loggers[name] = new StandardLogger(LogManager.GetLogger(name));

            return _loggers[name];
        }

        public string Trace(string inputMessage, Exception exception)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Trace(message);
            return message;
        }

        public string Info(string inputMessage, Exception exception)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Info(message);
            return message;
        }

        public string Debug(string inputMessage, Exception exception = null)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Debug(message);
            return message;
        }

        public string Warning(string inputMessage, Exception exception = null)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Warn(message);
            return message;
        }

        public string Error(string inputMessage, Exception exception = null)
        {
            string message = exception == null ? inputMessage : $"{inputMessage}{exception}";
            _logger.Error(message);
            return message;
        }

        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsWarningEnabled => _logger.IsWarnEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;
    }
}