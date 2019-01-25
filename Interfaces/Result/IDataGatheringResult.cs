using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace Interfaces.Result
{
    public interface IDataGatheringResult : IResult
    {
        IToolSpecification SpecificationRepository { get; }

        IReferenceSample ReferenceRepository { get; }

        List<IToolMeasurementData> MeasurementDataRepository { get; }

    }
}
