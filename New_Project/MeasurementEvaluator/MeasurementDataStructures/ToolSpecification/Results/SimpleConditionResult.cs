using Interfaces.MeasurementEvaluator.ReferenceSample;
using Interfaces.MeasurementEvaluator.Result;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace MeasurementDataStructures.ToolSpecification.Results
{
    internal class SimpleConditionEvaluationResult : ConditionEvaluationResultBase
    {
        public SimpleConditionEvaluationResult(DateTime creationTime, ICondition condition, IReferenceValue referenceValue, bool conditionIsMet, ICalculationResult calculationResult)
            : base(creationTime, condition, referenceValue, conditionIsMet, calculationResult)
        { }


    }
}
