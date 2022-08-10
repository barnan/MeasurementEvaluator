using Frame.PluginLoader;
using FrameInterfaces;
using Interfaces.Misc;

namespace DateTimeProviders
{
    internal sealed class StandardDateTimeProvider : IDateTimeProvider
    {
        IMyLogger _logger;

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
            _logger = PluginLoader.GetLogger(name);

            _logger.Info("Instantiated.");
        }
    }
}
