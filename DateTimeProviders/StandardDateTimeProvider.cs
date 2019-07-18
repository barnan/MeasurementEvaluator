using Interfaces.Misc;
using NLog;
using System;

namespace DateTimeProviders
{
    internal sealed class StandardDateTimeProvider : IDateTimeProvider
    {
        ILogger _logger;

        public DateTime GetDateTime()
        {
            DateTime datetime = DateTime.Now;

            if (_logger.IsTraceEnabled)
            {
                _logger.Trace($"DateTime required -> Returning DateTime: {datetime}");
            }

            return datetime;
        }

        public StandardDateTimeProvider(string name)
        {
            _logger = LogManager.GetLogger(name);

            _logger.Info("Instantiated.");
        }
    }
}
