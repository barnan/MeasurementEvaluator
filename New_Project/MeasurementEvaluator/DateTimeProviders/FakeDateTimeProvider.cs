using Frame.Configuration;
using Frame.PluginLoader;
using FrameInterfaces;
using Interfaces.Misc;

namespace DateTimeProviders
{
    /// <summary>
    /// Returns always the same parameter given time point
    /// </summary>
    internal sealed class FakeDateTimeProvider : IDateTimeProvider
    {
        private readonly FakeDateTimeProviderParameter _parameter;

        public DateTime GetDateTime()
        {
            if (_parameter.Logger.IsTraceEnabled)
            {
                _parameter.Logger.Trace($"DateTime required -> Returning DateTime: {_parameter.DateTimeToUse}");
            }

            return _parameter.DateTimeToUse;
        }

        internal FakeDateTimeProvider(FakeDateTimeProviderParameter parameter)
        {
            _parameter = parameter;

            _parameter.Logger.Info($"Instantiated. Parameter DateTime: {_parameter.DateTimeToUse}");
        }
    }


    internal sealed class FakeDateTimeProviderParameter
    {
        [Configuration("DateTimeToUse", "DateTimeToUse")]
        private DateTime _dateTimeToUse = DateTime.Now;
        internal DateTime DateTimeToUse => _dateTimeToUse;

        internal IMyLogger Logger { get; set; }

        internal bool Load(string sectionName)
        {
            Logger = PluginLoader.GetLogger(sectionName);

            PluginLoader.ConfigManager.Load(this, sectionName);

            return true;
        }
    }
}
