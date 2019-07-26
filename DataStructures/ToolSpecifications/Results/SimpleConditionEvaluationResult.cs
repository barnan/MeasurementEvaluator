using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;

namespace DataStructures.ToolSpecifications.Results
{
    internal class SimpleConditionEvaluationResult : ConditionEvaluationResult
    {
        public SimpleConditionEvaluationResult(DateTime creationTime, ICondition condition, IReferenceValue referenceValue, bool conditionIsMet, ICalculationResult calculationResult)
            : base(creationTime, condition, referenceValue, conditionIsMet, calculationResult)
        { }


    }
}
