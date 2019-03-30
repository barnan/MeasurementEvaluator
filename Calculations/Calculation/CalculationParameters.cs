using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Misc;
using NLog;

namespace Calculations.Calculation
{
    internal class CalculationParameters
    {
        public ILogger Logger { get; private set; }


        [Configuration("DateTimeProvider", "DateTimeProvider", LoadComponent = true)]
        private IDateTimeProvider _dateTimeProvider = null;
        public IDateTimeProvider DateTimeProvider => _dateTimeProvider;


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (DateTimeProvider == null)
            {
                Logger.Error($"Error in the {nameof(CalculationParameters)} instantiation. {nameof(DateTimeProvider)} is null.");
                return false;
            }

            return true;
        }
    }
}
