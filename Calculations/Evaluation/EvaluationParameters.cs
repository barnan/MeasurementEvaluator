using Interfaces.Calculation;
using Interfaces.DataAcquisition;
using Interfaces.Misc;
using NLog;

namespace Calculations.Evaluation
{
    internal class EvaluationParameters
    {
        internal ILogger Logger { get; }

        internal ICalculationContainer CalculationContainer { get; }

        internal IDataCollector DataCollector { get; }

        internal IDateTimeProvider DateTimeProvider { get; }


        internal EvaluationParameters(ICalculationContainer calculationContainer, IDataCollector dataCollector, IDateTimeProvider datetimeProvider)
        {
            Logger = LogManager.GetCurrentClassLogger();
            CalculationContainer = calculationContainer;
            DataCollector = dataCollector;
            DateTimeProvider = datetimeProvider;
        }

    }
}
