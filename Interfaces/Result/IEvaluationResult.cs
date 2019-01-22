using Interfaces.ReferenceSample;

namespace Interfaces.Result
{
    interface IEvaluationResult : IResult
    {
        ICalculationResult EvaluatedCalculationResult { get; }

        IReferenceSample ReferenceSample { get; }
    }
}
