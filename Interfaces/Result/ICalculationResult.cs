using Interfaces.MeasuredData;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace Interfaces.Result
{


    public interface IConditionCalculationResult : IResult
    {
        IToolMeasurementData MeasurementData { get; }

        ICondition Condition { get; }
    }

    // TODO: hash identification for the condition?


    public interface IConditionCalculationResult<out T> : IConditionCalculationResult where T : struct
    {
        T Results { get; }

        bool ConditionIsMet { get; }
    }


    public interface ICalculationResult : IResult
    {
        IReadOnlyList<IConditionCalculationResult> CalculationResults { get; }

    }

}
