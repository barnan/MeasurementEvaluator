using Interfaces.ReferenceSample;
using System.Collections.Generic;

namespace Interfaces.Result
{

    public interface IConditionEvaluationResult : IResult
    {
        IConditionCalculationResult CalculationResult { get; }

        IReferenceValue ReferenceValue { get; }

        bool ConditionIsMet { get; }
    }



    public interface IQuantityEvaluationResult
    {
        IReadOnlyList<IConditionCalculationResult> ConditionEvaluationResults { get; }
    }



    public interface IEvaluationResult : IResult
    {
        IReadOnlyList<IQuantityCalculationResult> EvaluationResults { get; }
    }
}
