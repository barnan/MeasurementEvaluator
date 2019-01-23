using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    public interface IMeasurementSerie
    {
        /// <summary>
        /// Name of the measured quantity
        /// </summary>
        string MeasuredQuantityName { get; }

        /// <summary>
        /// List of measurement result
        /// </summary>
        IReadOnlyList<IMeasurementPoint> MeasData { get; }

        /// <summary>
        /// indexer to get a data point with a given index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        IMeasurementPoint this[int i] { get; }

        /// <summary>
        /// Unit of the measurement
        /// </summary>
        Units Dimension { get; }
    }
}
