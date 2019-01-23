using Interfaces.Calculation;
using NLog;

namespace Calculations.Calculation
{
    internal class CalculationParameters
    {
        public ILogger Logger { get; }

        public ICalculationContainer CalculationContainer { get; }



        public CalculationParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();

            CalculationContainer = new CalculationContainer(new CalculationContainerParameters());
        }

    }
}
