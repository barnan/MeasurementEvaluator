using Frame.PluginLoader;
using FrameInterfaces;
using Interfaces.Misc;

namespace DateTimeProviders
{
    /// <summary>
    /// Returns the current (DateTime-Now) time
    /// </summary>
    internal sealed class SimpleDateTimeProvider : IDateTimeProvider
    {
        private readonly IMyLogger _logger;

        public DateTime GetDateTime()
        {
            DateTime datetime = DateTime.Now;

            if (_logger.IsTraceEnabled)
            {
                _logger.Trace($"DateTime required -> Returning DateTime: {datetime}");
            }

            return datetime;
        }

        public SimpleDateTimeProvider(string name)
        {
            _logger = PluginLoader.GetLogger(name);

            _logger.Info("Instantiated");
        }
    }
}
