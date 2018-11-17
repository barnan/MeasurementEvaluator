using System.Collections.Generic;

namespace Interfaces.MeasuredData
{
    public interface IMeasurementSeriesData
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
        /// Unit of the measurement
        /// </summary>
        Units Dimension { get; set; }
    }
}
