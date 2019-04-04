using DataStructures;
using Interfaces.Result;
using System;

namespace Calculations.Calculation
{
    class CalculationResult : ResultBase, ICalculationResult
    {

        public CalculationResult(DateTime startTime, DateTime endTime, bool successful)
        : base(startTime, endTime, successful)
        {
        }

    }
}
