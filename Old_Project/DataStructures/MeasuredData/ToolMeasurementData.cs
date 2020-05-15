using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class ToolMeasurementData : IToolMeasurementData
    {
        public ToolNames ToolName { get; set; }

        public IReadOnlyList<IMeasurementSerie> Results { get; set; }

        public string Name { get; set; }


        public ToolMeasurementData()
        {
        }

    }
}
