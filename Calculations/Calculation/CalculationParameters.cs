using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces;
using Interfaces.Misc;
using NLog;

namespace Calculations.Calculation
{
    internal class CalculationParameters
    {
        public ILogger Logger { get; private set; }


        [Configuration("DateTimeProvider", "DateTimeProvider", LoadComponent = true)]
        private string _dateTimeProviderName;
        public IDateTimeProvider DateTimeProvider;


        [Configuration("CalculationType", "CalculationType", LoadComponent = true)]
        private CalculationTypes _calculationType = CalculationTypes.Unknown;
        public CalculationTypes CalculationType => _calculationType;


        internal bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);
            DateTimeProvider = PluginLoader.CreateInstance<IDateTimeProvider>(_dateTimeProviderName);

            return CheckComponents();
        }

        private bool CheckComponents()
        {
            if (DateTimeProvider == null)
            {
                Logger.Error($"Error in the {nameof(CalculationParameters)} loading. {nameof(DateTimeProvider)} is null.");
                return false;
            }

            return true;
        }
    }
}
