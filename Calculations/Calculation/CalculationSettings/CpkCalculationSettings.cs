using Interfaces;
using Interfaces.Calculation;

namespace Calculations.Calculation.CalculationSettings
{
    internal class CpkCalculationSettings : CpCalculationSettings, ICpkCalculationSettings
    {
        public CpkCalculationSettings(CalculationTypes calculationType, double halfTolerance, double referenceValue)
            : base(calculationType, halfTolerance)
        {
            ReferenceValue = referenceValue;
        }

        public double ReferenceValue { get; }
    }
}
