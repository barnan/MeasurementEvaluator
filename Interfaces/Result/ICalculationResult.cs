using Interfaces.MeasuredData;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace Interfaces.Result
{

    public interface IConditionCalculationResult : IResult
    {
        IMeasurementSerie MeasurementSerieData { get; }

        ICondition Condition { get; }
    }

    // TODO: hash identification for the condition?


    public interface IConditionCalculationResult<out T> : IConditionCalculationResult where T : struct
    {
        T Results { get; }
    }


    public interface IQuantityCalculationResult
    {
        IReadOnlyList<IConditionCalculationResult> ConditionCalculationResults { get; }
    }


    public interface ICalculationResult : IResult
    {
        IReadOnlyList<IQuantityCalculationResult> CalculationResults { get; }

    }

}
