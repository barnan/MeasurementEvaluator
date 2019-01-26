using Interfaces.Result;
using System;

namespace Calculations.Calculation
{
    internal class SimpleCalculationResult : CalculationResult, ISimpleCalculationResult
    {
        public double Result { get; }


        public SimpleCalculationResult(double result, DateTime startTime, DateTime endTime, bool successful)
            : base(startTime, endTime, successful)
        {
            Result = result;
        }


    }
}
