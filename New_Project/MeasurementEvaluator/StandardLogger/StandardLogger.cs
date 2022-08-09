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

        public string Trace(string input, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public string Info(string input, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public string Debug(string input, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public string Warning(string input, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public string Error(string input, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsWarningEnabled => _logger.IsWarnEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;
    }
}