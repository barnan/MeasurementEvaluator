using DataStructures;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;

namespace MeasurementEvaluator.ME_DataCollector
{
    internal class DataCollectorResult : ResultBase, IDataCollectorResult
    {
        public IToolSpecification Specification { get; }

        public IReferenceSample Reference { get; }

        public IReadOnlyList<IToolMeasurementData> MeasurementData { get; }


        public DataCollectorResult(DateTime endTime, bool successfulCalculation, IToolSpecification toolSpecification, IReadOnlyList<IToolMeasurementData> measurementData, IReferenceSample referenceSample = null)
        : base(endTime, successfulCalculation)
        {
            Specification = toolSpecification;
            Reference = referenceSample;
            MeasurementData = measurementData;
        }
    }
}
