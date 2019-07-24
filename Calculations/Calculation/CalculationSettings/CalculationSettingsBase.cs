using Interfaces;
using Interfaces.BaseClasses;
using Interfaces.Calculation;

namespace Calculations.Calculation.CalculationSettings
{
    internal class CalculationSettingsBase : ICalculationSettings
    {
        public CalculationTypes ValidCalculation { get; }

        public CalculationSettingsBase(CalculationTypes calculationType)
        {
            ValidCalculation = calculationType;
        }
    }
}
