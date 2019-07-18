namespace Interfaces.Result
{

    public interface ICalculationResult<T> : IResult
    {/// <summary>
     /// calculation result
     /// </summary>
        T ResultValue { get; }
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
