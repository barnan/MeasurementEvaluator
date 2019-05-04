using Interfaces;
using Interfaces.Calculation;

namespace Calculations.Calculation.CalculationSettings
{
    internal class CpkCalculationSettings : CalculationSettingsBase, ICpkCalculationSettings
    {
        public CpkCalculationSettings(CalculationTypes calculationType, double halfTolerance, double referenceValue)
            : base(calculationType)
        {
            HalfTolerance = halfTolerance;
            ReferenceValue = referenceValue;
        }

        public double HalfTolerance { get; }

        public double ReferenceValue { get; }
    }
}
