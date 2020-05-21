﻿using BaseClasses;

namespace ToolSpecificInterfaces.MeasurementEvaluator.ToolSpecifications
{
    public interface ISimpleCondition : ICondition<double>
    {
        /// <summary>
        /// The condition is valid if the average value of the measurement data meets this relation
        /// </summary>
        Relations ValidIf { get; }

        /// <summary>
        /// The condition is valid if the absolute value of the measurement data meets this Validif condition with the ValidIfValue value
        /// </summary>
        double ValidIf_Value { get; }
    }


    public interface ISimpleConditionHandler : ISimpleCondition
    {
        new Relations ValidIf { get; set; }

        new double ValidIf_Value { get; set; }
    }

}
