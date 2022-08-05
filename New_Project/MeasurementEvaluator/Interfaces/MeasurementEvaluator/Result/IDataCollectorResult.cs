using Interfaces.MeasurementEvaluator.MeasuredData;
using Interfaces.MeasurementEvaluator.ReferenceSample;
using Interfaces.MeasurementEvaluator.ToolSpecification;

namespace Interfaces.MeasurementEvaluator.Result
{
    public interface IDataCollectorResult : IResult
    {
        /// <summary>
        /// The selected (loaded) toolspecification
        /// </summary>
        IToolSpecification Specification { get; }

        /// <summary>
        /// The selected (loaded) reference sample data
        /// </summary>
        IReferenceSample Reference { get; }

        /// <summary>
        /// The selected (loaded) measurement data file contents
        /// </summary>
        IReadOnlyList<IToolMeasurementData> MeasurementData { get; }

    }
}
