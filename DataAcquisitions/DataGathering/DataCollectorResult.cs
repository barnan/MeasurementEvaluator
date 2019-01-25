using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.DataGathering
{
    internal class DataCollectorResult : IDataCollectorResult
    {
        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public bool SuccessfulCalculation { get; }

        public IToolSpecification Specification { get; }

        public IReferenceSample Reference { get; }

        public IReadOnlyList<IToolMeasurementData> MeasurementData { get; }


        public DataCollectorResult(DateTime startTime, DateTime endTime, bool successfulCalculation, IToolSpecification toolSpecification, IReadOnlyList<IToolMeasurementData> measurementData, IReferenceSample referenceSample = null)
        {
            StartTime = startTime;
            EndTime = EndTime;
            SuccessfulCalculation = successfulCalculation;
            Specification = toolSpecification;
            Reference = referenceSample;
            MeasurementData = measurementData;
        }
    }
}
