using Interfaces.Misc;
using System;
using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    /// <summary>
    /// interface that describes the measurement result of one tool, only Getters
    /// </summary>
    public interface IToolMeasurementData : IComparable<IToolMeasurementData>, INamed
    {
        /// <summary>
        /// Name of the tool, which was used to create the measurement data (e.g. TTR, WSI, etc))
        /// </summary>
        ToolNames ToolName { get; }

        /// <summary>
        /// collection of measurement result of a given tool 
        /// (etc -> thickness, resistivity, sawmark are results of tool TTR)
        /// </summary>
        IReadOnlyList<IMeasurementSerie> Results { get; }
    }

    /// <summary>
    /// interface that describes the measurement result of one tool, Setters added
    /// </summary>
    public interface IToolMeasurementDataHandler : IToolMeasurementData, INamedHandler
    {
        new ToolNames ToolName { get; set; }

        new IReadOnlyList<IMeasurementSerie> Results { get; set; }
    }

    /// <summary>
    /// interface that describes the measurement result of more than one tools
    /// </summary>
    public interface IMeasurementDatas
    {
        IReadOnlyList<IToolMeasurementData> MeasurementDatas { get; }
    }

}
