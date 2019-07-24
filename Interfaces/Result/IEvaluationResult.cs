using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;

namespace Interfaces.Result
{

    public interface IConditionEvaluationResult : IResult, IFormattable
    {
        /// <summary>
        /// the result is calculated from this measuremnt series data
        /// </summary>
        IMeasurementSerie MeasurementSerieData { get; }

        /// <summary>
        /// the condition which is used in the evaluation of this result
        /// </summary>
        ICondition Condition { get; }

        /// <summary>
        /// the reference value which is used in the evaluation of this result
        /// </summary>
        IReferenceValue ReferenceValue { get; }

        /// <summary>
        /// result of the evaluation. The condition is whether true or not
        /// </summary>
        bool ConditionIsMet { get; }

        /// <summary>
        /// The number result of the calculation
        /// </summary>
        IResult Result { get; }
    }



    public interface IQuantityEvaluationResult
    {
        IReadOnlyList<IConditionEvaluationResult> ConditionEvaluationResults { get; }
    }



    public interface IEvaluationResult : IResult
    {
        IReadOnlyList<IQuantityEvaluationResult> QuantityEvaluationResults { get; }
    }
}
