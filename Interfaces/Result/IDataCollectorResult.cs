using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace Interfaces.Result
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
