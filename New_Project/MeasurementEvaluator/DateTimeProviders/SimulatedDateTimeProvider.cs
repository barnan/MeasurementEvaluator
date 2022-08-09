using FrameInterfaces;
using Interfaces.Misc;

namespace DateTimeProviders
{
    internal sealed class SimulatedDateTimeProvider : IDateTimeProvider
    {
        SimulatedDateTimeProviderParameter _parameter;
        DateTime _instantiationDateTime;


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
        public DateTime DateTimeToStart { get; }

        internal IMyLogger Logger { get; private set; }


        internal bool Load(string sectionName)
        {
            PluginLoader.ConfigManager.Load(this, sectionName);

            Logger = LogManager.GetLogger(sectionName);

            return true;
        }
    }
}
