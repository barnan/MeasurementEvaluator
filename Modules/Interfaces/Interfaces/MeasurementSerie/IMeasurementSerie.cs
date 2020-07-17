using BaseClasses;
using Interfaces.Misc;
using System.Collections.Generic;

namespace Interfaces.MeasurementSerie
{
    public interface IMeasurementSerie : INamed
    {
        /// <summary>
        /// List of measurement result
        /// </summary>
        List<INumericMeasurementPoint> MeasuredPoints { get; }

        /// <summary>
        /// indexer to get a data point with a given index
        /// </summary>
        /// <param name="i">input index</param>
        /// <returns></returns>
        INumericMeasurementPoint this[int i] { get; }

        /// <summary>
        /// Unit of the measurement data
        /// </summary>
        Units Dimension { get; }
    }

}
