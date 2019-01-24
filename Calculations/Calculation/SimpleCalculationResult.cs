using Interfaces.Result;
using System;

namespace DataStructures.Calculation
{
    public class SimpleCalculationResult : ISimpleCalculationResult
    {
        public DateTime CreationTime { get; }

        public double Result { get; }


        // TODO: time provider?
        public SimpleCalculationResult(DateTime creationTime, double result)
        {
            CreationTime = creationTime;
            Result = result;
        }
    }
}
