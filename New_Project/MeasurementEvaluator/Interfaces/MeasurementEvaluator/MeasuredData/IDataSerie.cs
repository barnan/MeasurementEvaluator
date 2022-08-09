using BaseClasses.MeasurementEvaluator;
using Interfaces.Misc;

namespace Interfaces.MeasurementEvaluator.MeasuredData
{
    public interface IDataSerie : INamed 
    {
        /// <summary>
        /// List of measurement result
        /// </summary>
        IReadOnlyList<IDataPoint> DataPoints { get; }

        /// <summary>
        /// indexer to get a data point with a given index
        /// </summary>
        /// <param name="i">input index</param>
        /// <returns></returns>
        IDataPoint this[int i] { get; }

        /// <summary>
        /// Unit of the measurement data
        /// </summary>
        Units Dimension { get; }
    }

    public interface IDataSerie<T> : IDataSerie
    {
        /// <summary>
        /// List of measurement result
        /// </summary>
        new IReadOnlyList<IDataPoint<T>> DataPoints { get; }

        /// <summary>
        /// indexer to get a data point with a given index
        /// </summary>
        /// <param name="i">input index</param>
        /// <returns></returns>
        new IDataPoint<T> this[int i] { get; }
    }
}
