using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.DataGathering
{
    internal class DataGatheringResult : IDataGatheringResult
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public bool SuccessfulCalculation { get; }

        public IToolSpecification SpecificationRepository { get; }

        public IReferenceSample ReferenceRepository { get; }

        public List<IToolMeasurementData> MeasurementDataRepository { get; }
    }
}
