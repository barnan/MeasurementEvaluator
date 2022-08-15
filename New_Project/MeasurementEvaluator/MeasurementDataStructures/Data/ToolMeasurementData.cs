using Interfaces;
using Interfaces.MeasurementEvaluator.MeasuredData;

namespace MeasurementDataStructures.Data
{
    public class ToolMeasurementData : IToolMeasurementData
    {
        public ToolMeasurementData(ToolNames toolName, IReadOnlyList<IMeasurementSerie> results, string name)
        {
            ToolName = toolName;
            Results = results;
            Name = name;
        }

        public ToolNames ToolName { get; set; }

        public IReadOnlyList<IMeasurementSerie> Results { get; set; }

        public string Name { get; set; }


    }
}
