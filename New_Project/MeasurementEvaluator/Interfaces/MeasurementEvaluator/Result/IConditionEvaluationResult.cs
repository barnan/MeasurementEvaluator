﻿using Interfaces.MeasurementEvaluator.ReferenceSample;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace Interfaces.MeasurementEvaluator.Result
{
    public interface IConditionEvaluationResult : IResult, IFormattable
    {
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
        ICalculationResult CalculationResult { get; }
    }
}
