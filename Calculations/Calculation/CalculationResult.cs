using DataStructures;
using Interfaces.Result;
using System;

namespace Calculations.Calculation
{
    class CalculationResult<T> : ResultBase, ICalculationResult<T>
    {
        public CalculationResult(DateTime creationTime, bool successful)
            : base(creationTime, successful)
        {
        }

        public T ResultValue { get; protected set; }
    }
}
