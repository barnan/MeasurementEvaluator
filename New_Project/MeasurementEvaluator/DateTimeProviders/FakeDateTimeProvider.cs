using Frame.PluginLoader;
using FrameInterfaces;
using Interfaces.Misc;

namespace DateTimeProviders
{
    internal sealed class FakeDateTimeProvider : IDateTimeProvider
    {
        FakeDateTimeProviderParameter _parameter;

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
        private DateTime _dateTimeToUse = default(DateTime);
        internal DateTime DateTimeToUse => _dateTimeToUse;

        internal IMyLogger Logger { get; private set; }

        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            Logger = PluginLoader.GetLogger(sectionName);

            return true;
        }
    }
}
