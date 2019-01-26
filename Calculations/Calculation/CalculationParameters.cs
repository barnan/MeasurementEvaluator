using Interfaces.Misc;
using Miscellaneous;
using NLog;

namespace Calculations.Calculation
{
    internal class CalculationParameters
    {
        public ILogger Logger { get; }

        //public ICalculationContainer CalculationContainer { get; }

        public IDateTimeProvider DateTimeProvider { get; }



        public CalculationParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();

            //CalculationContainer = new CalculationContainer(new CalculationContainerParameters());

            DateTimeProvider = new StandardDateTimeProvider(new StandardDateTimeProviderParameter());
        }

    }
}
