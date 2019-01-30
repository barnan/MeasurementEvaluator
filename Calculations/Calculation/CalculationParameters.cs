using Interfaces.Misc;
using Miscellaneous;
using NLog;

namespace Calculations.Calculation
{
    internal class CalculationParameters
    {
        public ILogger Logger { get; }

        public IDateTimeProvider DateTimeProvider { get; }



        public CalculationParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();

            DateTimeProvider = new StandardDateTimeProvider(new StandardDateTimeProviderParameter());
        }

    }
}
