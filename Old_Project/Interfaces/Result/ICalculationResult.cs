using Interfaces.MeasuredData;

namespace Interfaces.Result
{

    public interface ICalculationResult : IResult
    {
        /// <summary>
        /// the data serie which was used in the calculation
        /// </summary>
        IMeasurementSerie MeasurementSerie { get; }
    }



    public interface ICalculationResult<T> : ICalculationResult
    {
        /// <summary>
        /// calculation result
        /// </summary>
        T ResultValue { get; }

        T Average { get; }
    }



    public interface ISimpleCalculationResult : ICalculationResult<double>
    {
    }


    public interface IQCellsCalculationResult : ICalculationResult<double>
    {
        /// <summary>
        /// Upper Specification Limit
        /// </summary>
        double USL { get; }


        /// <summary>
        /// Lower Specification Limit
        /// </summary>
        double LSL { get; }
    }



    public interface IGRAndRCalculationResult : ICalculationResult<double>
    {
    }

}
