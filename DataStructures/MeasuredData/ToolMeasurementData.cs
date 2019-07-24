using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using System;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class ToolMeasurementData : IToolMeasurementData, IComparable<IToolMeasurementData>
    {
        public ToolNames ToolName { get; set; }

        public IReadOnlyList<IMeasurementSerie> Results { get; set; }

        public string Name { get; set; }


        public ToolMeasurementData()
        {
        }


        // TODO refactor this method
        public int CompareTo(IToolMeasurementData other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            var toolNameComparison = (ToolName.Value - other.ToolName.Value) / Math.Abs(ToolName.Value - other.ToolName.Value);

            if (toolNameComparison != 0) return toolNameComparison;

            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}
