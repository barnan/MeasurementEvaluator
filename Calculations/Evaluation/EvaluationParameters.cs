using Interfaces.Calculation;
using NLog;

namespace Calculations.Evaluation
{
    class EvaluationParameters
    {
        public ILogger Logger { get; private set; }
        public ICalculation Calculation { get; }


        public EvaluationParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

    }
}
