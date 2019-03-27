using Frame.ConfigHandler;
using Frame.PluginLoader;
using Interfaces.Calculation;
using Interfaces.DataAcquisition;
using Interfaces.Evaluation;
using Interfaces.Misc;
using NLog;

namespace Calculations.Evaluation
{
    internal class EvaluationParameters
    {
        internal ILogger Logger { get; private set; }

        //[Configuration("Contains the possible calculation", Name = "Name of Calculation Container Component", LoadComponent = true)]
        //private ICalculationContainer _calculationContainer;
        //internal ICalculationContainer CalculationContainer
        //{
        //    get => _calculationContainer;
        //    private set => _calculationContainer = value;
        //}

        //[Configuration("Data Collection", Name = "Name of Data collector Component", LoadComponent = true)]
        //private IDataCollector _dataCollector;
        //internal IDataCollector DataCollector
        //{
        //    get => _dataCollector;
        //    private set => _dataCollector = value;
        //}

        //[Configuration("Date and time provider", Name = "Name of the DateTimeProvider Component", LoadComponent = true)]
        //private IDateTimeProvider _dateTimeProvider;
        //internal IDateTimeProvider DateTimeProvider
        //{
        //    get => _dateTimeProvider;
        //    private set => _dateTimeProvider = value;
        //}

        //[Configuration("Data matching", Name = "Name of the Data matcher Component", LoadComponent = true)]
        //private IMathing _matcher;
        //internal IMathing Matcher
        //{
        //    get => _matcher;
        //    private set => _matcher = value;
        //}


        [Configuration("Contains the possible calculation", Name = "Name of Calculation Container Component", LoadComponent = true)]
        private string _calculationContainer = null;
        internal ICalculationContainer CalculationContainer { get; private set; }

        [Configuration("Data Collection", Name = "Name of Data collector Component", LoadComponent = true)]
        private string _dataCollector = null;
        internal IDataCollector DataCollector { get; private set; }

        [Configuration("Date and time provider", Name = "Name of the DateTimeProvider Component", LoadComponent = true)]
        private string _dateTimeProvider = null;
        internal IDateTimeProvider DateTimeProvider { get; private set; }

        [Configuration("Data matching", Name = "Name of the Data matcher Component", LoadComponent = true)]
        private string _matcher = null;
        internal IMathing Matcher { get; private set; }


        public bool Load(string sectionName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            PluginLoader.ConfigManager.Load(this, sectionName);

            CalculationContainer = PluginLoader.CreateInstance<ICalculationContainer>(_calculationContainer);
            DataCollector = PluginLoader.CreateInstance<IDataCollector>(_dataCollector);
            DateTimeProvider = PluginLoader.CreateInstance<IDateTimeProvider>(_dateTimeProvider);
            Matcher = PluginLoader.CreateInstance<IMathing>(_matcher);

            return CheckComponent();
        }

        private bool CheckComponent()
        {
            if (CalculationContainer == null)
            {
                Logger.Error($"Error in the {nameof(EvaluationParameters)} instantiation. {nameof(CalculationContainer)} is null.");
                return false;
            }

            if (DataCollector == null)
            {
                Logger.Error($"Error in the {nameof(EvaluationParameters)} instantiation. {nameof(DataCollector)} is null.");
                return false;
            }

            if (DateTimeProvider == null)
            {
                Logger.Error($"Error in the {nameof(EvaluationParameters)} instantiation. {nameof(DateTimeProvider)} is null.");
                return false;
            }

            if (Matcher == null)
            {
                Logger.Error($"Error in the {nameof(EvaluationParameters)} instantiation. {nameof(Matcher)} is null.");
                return false;
            }

            return true;
        }
    }
}
