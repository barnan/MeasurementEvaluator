using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    public interface IMeasurementSerieData
    {
        /// <summary>
        /// Name of the measured quantity
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// List of measurement result
        /// </summary>
        List<IIndividualMeasurementResult> MeasData { get; set; }

        /// <summary>
        /// indexer to get a data point with a given index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        IIndividualMeasurementResult this[int i] { get; }

        /// <summary>
        /// Unit of the measurement
        /// </summary>
        Units Dimension { get; set; }
    }
}
