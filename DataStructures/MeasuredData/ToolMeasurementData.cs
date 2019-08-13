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


        public int CompareTo(IToolMeasurementData other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return 0;
            }

            if (object.ReferenceEquals(null, other))
            {
                return 1;
            }

            return ToolName.Value - other.ToolName.Value;
        }
    }
}
