using Interfaces.Calculation;
using Interfaces.DataAcquisition;
using Interfaces.Evaluation;
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

        internal IMathing Matcher { get; }

        internal EvaluationParameters(ICalculationContainer calculationContainer, IDataCollector dataCollector, IDateTimeProvider datetimeProvider, IMathing mathing)
        {
            Logger = LogManager.GetCurrentClassLogger();
            CalculationContainer = calculationContainer;
            DataCollector = dataCollector;
            DateTimeProvider = datetimeProvider;
            Matcher = mathing;
        }

    }
}
