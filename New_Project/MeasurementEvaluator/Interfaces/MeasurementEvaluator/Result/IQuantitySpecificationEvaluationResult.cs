using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace Interfaces.MeasurementEvaluator.Result
{
    public interface IQuantitySpecificationEvaluationResult : IResult, IFormattable
    {
        IReadOnlyList<IConditionEvaluationResult> ConditionEvaluationResults { get; }

        IQuantity Quantity { get; }
    }
}
