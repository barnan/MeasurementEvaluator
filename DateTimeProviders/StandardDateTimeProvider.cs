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

        public StandardDateTimeProvider()
        {
            _logger = LogManager.GetCurrentClassLogger();

            _logger.Info("Instantiated.");
        }
    }
}
