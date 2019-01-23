using Interfaces.Calculation;
using NLog;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    internal class CalculationContainerParameters
    {
        private List<ICalculation> _availableCalculations = new List<ICalculation> { new AverageCalculation1D(new CalculationParameters()),
                                                                                     new StdCalculation1D(new CalculationParameters()) };
        public List<ICalculation> AvailableCalculations => _availableCalculations;


        public ILogger Logger { get; }


        public CalculationContainerParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

    }
}
