using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
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
        List<IMeasurementSerie> Results { get; }
    }



    public interface IToolMeasurementDataHandler : IToolMeasurementData, IStoredDataOnHDDHandler
    {

        string ToolName { get; set; }

        List<IMeasurementSerie> Results { get; set; }
    }


}
