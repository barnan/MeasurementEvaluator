using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    /// <summary>
    /// interface that describes the measurement result of one tool, only Geters
    /// </summary>
    public interface IToolMeasurementData : IStoredDataOnHDD, IComparable<IToolMeasurementData>
    {

        /// <summary>
        /// Name of the tool, which was used to prepare the measurement data
        /// </summary>
        string ToolName { get; }

        /// <summary>
        /// collection of measurement result of a given tool 
        /// (etc -> thickness, resistivity, sawmark are results of TTR)
        /// </summary>
        IReadOnlyList<IMeasurementSerie> Results { get; }
    }


    /// <summary>
    /// interface that describes the measurement result of one tool, Setters added
    /// </summary>
    public interface IToolMeasurementDataHandler : IToolMeasurementData, IStoredDataOnHDDHandler
    {

        string ToolName { get; set; }

        IReadOnlyList<IMeasurementSerie> Results { get; set; }
    }



    /// <summary>
    /// interface that describes the measurement result of more than one tools
    /// </summary>
    public interface IMeasurementDatas
    {
        IReadOnlyList<IToolMeasurementData> MeasurementDatas { get; }
    }



}
