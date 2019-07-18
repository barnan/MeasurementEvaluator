using Interfaces.Misc;
using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    public interface IMeasurementSerie : INamed
    {
        /// <summary>
        /// List of measurement result
        /// </summary>
        IReadOnlyList<IMeasurementPoint> MeasuredPoints { get; }

        /// <summary>
        /// indexer to get a data point with a given index
        /// </summary>
        /// <param name="i">input index</param>
        /// <returns></returns>
        IMeasurementPoint this[int i] { get; }

        /// <summary>
        /// Unit of the measurement data
        /// </summary>
        Units Dimension { get; }
    }

}
