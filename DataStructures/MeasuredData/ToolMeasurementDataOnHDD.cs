using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class ToolMeasurementData : IToolMeasurementData
    {
        public string ToolName { get; }

        public List<IMeasurementSerie> Results { get; }

        public string FullNameOnHDD { get; }


        public ToolMeasurementData(string toolName, List<IMeasurementSerie> results)
        {
            ToolName = toolName;
            Results = results;
        }


    }
}
