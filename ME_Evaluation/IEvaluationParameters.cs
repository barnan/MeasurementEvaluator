using Frame.MessageHandler;
using Interfaces.Calculation;
using Interfaces.DataAcquisition;
using Interfaces.Evaluation;
using Interfaces.Misc;
using NLog;

namespace MeasurementEvaluator.ME_Evaluation
{
    internal interface IEvaluationParameters
    {
        ILogger Logger { get; }
        ICalculationContainer CalculationContainer { get; }
        IDataCollector DataCollector { get; }
        IDateTimeProvider DateTimeProvider { get; }
        IMatching Matcher { get; }
        IUIMessageControl MessageControl { get; }
        string Name { get; }
    }
}