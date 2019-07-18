using Interfaces.Result;
using System;

namespace Calculations.Calculation
{
    internal class SimpleCalculationResult : CalculationResult<double>, ISimpleCalculationResult
    {
        public double ResultValue { get; }


        public SimpleCalculationResult(double result, DateTime creationTime, bool successful)
            : base(creationTime, successful)
        {
            ResultValue = result;
        }


    }
}
