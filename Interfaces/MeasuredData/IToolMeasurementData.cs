using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    public interface IToolMeasurementData
    {

        /// <summary>
        /// collection of measurement result of a given tool 
        /// (etc -> thickness, resistivity, sawmark are results of TTR)
        /// </summary>
        List<IMeasurementSerie> Results { get; }

        /// <summary>
        /// indexer to get a given measurement series with a given index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        IMeasurementSerie this[int i] { get; }
    }

}
