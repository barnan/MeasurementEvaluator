using Interfaces.MeasurementEvaluator.ReferenceSample;
using Interfaces.Misc;

namespace Interfaces.MeasurementEvaluator.Result
{

    public interface IToolSpecificationEvaluationResult : IResult, INamed, IFormattable
    {
        IReadOnlyList<IQuantitySpecificationEvaluationResult> QuantityEvaluationResults { get; }

        ToolNames ToolName { get; }
    }
}
