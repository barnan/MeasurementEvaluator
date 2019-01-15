using Interfaces.MeasuredData;
using System;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class ToolMeasurementData : IToolMeasurementData, IComparable<IToolMeasurementData>
    {
        public string ToolName { get; set; }

        public List<IMeasurementSerie> Results { get; set; }

        public string FullNameOnHDD { get; set; }


        public ToolMeasurementData()
        {
        }


        // TODO refactor this method
        public int CompareTo(IToolMeasurementData other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var toolNameComparison = string.Compare(ToolName, other.ToolName, StringComparison.Ordinal);
            if (toolNameComparison != 0) return toolNameComparison;
            return string.Compare(FullNameOnHDD, other.FullNameOnHDD, StringComparison.Ordinal);
        }
    }
}
