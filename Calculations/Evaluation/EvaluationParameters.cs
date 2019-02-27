using Interfaces.Calculation;
using Interfaces.DataAcquisition;
using Interfaces.Evaluation;
using Interfaces.Misc;
using Miscellaneous;
using NLog;
using PluginLoading;

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
        private string _calculationContainer;
        internal ICalculationContainer CalculationContainer { get; private set; }

        [Configuration("Data Collection", Name = "Name of Data collector Component", LoadComponent = true)]
        private string _dataCollector;
        internal IDataCollector DataCollector { get; private set; }

        [Configuration("Date and time provider", Name = "Name of the DateTimeProvider Component", LoadComponent = true)]
        private string _dateTimeProvider;
        internal IDateTimeProvider DateTimeProvider { get; private set; }

        [Configuration("Data matching", Name = "Name of the Data matcher Component", LoadComponent = true)]
        private string _matcher;
        internal IMathing Matcher { get; private set; }




        public bool Load()
        {
            Logger = LogManager.GetCurrentClassLogger();

            CalculationContainer = PluginLoader.CreateInstance<ICalculationContainer>(_calculationContainer);
            DataCollector = PluginLoader.CreateInstance<IDataCollector>(_dataCollector);
            DateTimeProvider = PluginLoader.CreateInstance<IDateTimeProvider>(_dateTimeProvider);
            Matcher = PluginLoader.CreateInstance<IMathing>(_matcher);

            return true;
        }


    }
}
