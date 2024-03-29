﻿
namespace Interfaces.MeasurementEvaluator.ToolSpecification
{
    public interface ICpkCondition : ICondition
    {
        /// <summary>
        /// Half of the tolerance interval
        /// </summary>
        double HalfTolerance { get; }

    }

    public interface ICpkConditionHandler : ICpkCondition
    {
        new double HalfTolerance { get; set; }

    }
}
