using Interfaces.Result;
using System;

namespace Calculations.Calculation
{
    internal class QCellsCalculationResult : IQCellsCalculationResult
    {
        public DateTime CreationTime { get; }

        public double Result { get; }

        public double USL { get; }

        public double LSL { get; }


        public QCellsCalculationResult(DateTime creationTime, double result, double usl, double lsl)
        {
            CreationTime = creationTime;
            Result = result;
            USL = usl;
            LSL = lsl;
        }
    }
}
