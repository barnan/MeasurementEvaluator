namespace Interfaces.Result
{

    public interface ICalculationResult : IResult
    {
    }



    public interface ISimpleCalculationResult : ICalculationResult
    {
        /// <summary>
        /// calculation result
        /// </summary>
        double Result { get; }
    }


    public interface IQCellsCalculationResult : ICalculationResult
    {
        /// <summary>
        /// calculation result
        /// </summary>
        double Result { get; }

        /// <summary>
        /// Upper Specification Limit
        /// </summary>
        double USL { get; }


        /// <summary>
        /// Lower Specification Limit
        /// </summary>
        double LSL { get; }
    }



    public interface IGRAndRCalculationResult : ICalculationResult
    {
    }

}
