using NLog;

namespace Calculations.Calculation
{
    internal class CalculationParameters
    {
        public ILogger Logger { get; private set; }



        public CalculationParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }




    }
}
