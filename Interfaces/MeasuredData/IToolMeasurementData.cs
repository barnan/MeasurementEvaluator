using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    public interface IToolMeasurementData
    {

        /// <summary>
        /// Name of the tool, which was used to prepare the measurement data
        /// </summary>
        string ToolName { get; }

        /// <summary>
        /// collection of measurement result of a given tool 
        /// (etc -> thickness, resistivity, sawmark are results of TTR)
        /// </summary>
        List<IMeasurementSerie> Results { get; }

    }

}
