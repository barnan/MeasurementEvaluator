using Interfaces.Result;
using System;

namespace Calculations.Calculation
{
    internal class QCellsCalculationResult : CalculationResult, IQCellsCalculationResult
    {
        public double Result { get; }

        public double USL { get; }

        public double LSL { get; }


        public QCellsCalculationResult(double result, double usl, double lsl,
                                        DateTime startTime, DateTime endTime, bool successful)
            : base(startTime, endTime, successful)
        {
            Result = result;
            USL = usl;
            LSL = lsl;
        }

    }
}
