using Interfaces;
using Interfaces.BaseClasses;
using Interfaces.Calculation;

namespace Calculations.Calculation.CalculationSettings
{
    internal class CalculationSettingsBase : ICalculationSettings
    {
        public CalculationType ValidCalculation { get; }

        public CalculationSettingsBase(CalculationType calculationType)
        {
            ValidCalculation = calculationType;
        }
    }
}
