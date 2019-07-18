using Interfaces.Result;
using System;

namespace Calculations.Calculation
{
    internal class QCellsCalculationResult : CalculationResult<double>, IQCellsCalculationResult
    {
        public double USL { get; }

        public double LSL { get; }


        public QCellsCalculationResult(double result, double usl, double lsl, DateTime creationTime, bool successful)
            : base(creationTime, successful)
        {
            ResultValue = result;
            USL = usl;
            LSL = lsl;
        }

    }
}
