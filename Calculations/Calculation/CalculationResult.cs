using Interfaces.Result;
using Miscellaneous;
using System;

namespace Calculations.Calculation
{
    class CalculationResult : ResultBase, ICalculationResult
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public bool Successful { get; }



        public CalculationResult(DateTime startTime, DateTime endTime, bool successful)
        {
            StartTime = startTime;
            EndTime = endTime;
            Successful = successful;
        }

    }
}
