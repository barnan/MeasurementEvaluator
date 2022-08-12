using Frame.Configuration;
using Frame.PluginLoader;
using FrameInterfaces;
using Interfaces.Misc;

namespace DateTimeProviders
{
    /// <summary>
    /// Returns the elapsed time from a parameter given time-point
    /// </summary>
    internal sealed class SimulatedDateTimeProvider : IDateTimeProvider
    {
        private readonly SimulatedDateTimeProviderParameter _parameter;
        readonly DateTime _instantiationDateTime;


        public DateTime GetDateTime()
        {
            TimeSpan tillNowFromInstantiation = DateTime.Now - _instantiationDateTime;
            DateTime returnValue = _parameter.DateTimeToStart + tillNowFromInstantiation;

            if (_parameter.Logger.IsTraceEnabled)
            {
                _parameter.Logger.Trace($"DateTime required. Time elapsed from instantiation: {tillNowFromInstantiation}, Startup DateTime: {_parameter.DateTimeToStart}, Returning DateTime: {returnValue}");
            }

            return returnValue;
        }

        public SimulatedDateTimeProvider(SimulatedDateTimeProviderParameter parameter)
        {
            _parameter = parameter;
            _instantiationDateTime = DateTime.Now;

            _parameter.Logger.Info($"Instantiated. Parameter DateTime: {_parameter.DateTimeToStart}, current DateTime: {_instantiationDateTime}");
        }
    }


    internal sealed class SimulatedDateTimeProviderParameter
    {
        [Configuration("DateTimeToStart", "DateTimeToStart")]
        private DateTime _dateTimeToStart = DateTime.Now;
        internal DateTime DateTimeToStart => _dateTimeToStart;

        internal IMyLogger Logger { get; set; }


        internal bool Load(string sectionName)
        {
            Logger = PluginLoader.GetLogger(sectionName);

            PluginLoader.ConfigManager.Load(this, sectionName);

            return true;
        }
    }
}
