using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace Interfaces.Result
{

    public interface IConditionEvaluationResult : ISaveableResult
    {
        IMeasurementSerie MeasurementSerieData { get; }

        ICondition Condition { get; }

        IReferenceValue ReferenceValue { get; }

        bool ConditionIsMet { get; }

        ICalculationResult Result { get; }
    }



    public interface IQuantityEvaluationResult
    {
        IReadOnlyList<IConditionEvaluationResult> ConditionCalculationResults { get; }
    }



    public interface IEvaluationResult : IResult
    {
        IReadOnlyList<IQuantityEvaluationResult> EvaluationResults { get; }
    }
}
