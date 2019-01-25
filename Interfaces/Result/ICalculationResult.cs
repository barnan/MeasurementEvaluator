using System;

namespace Interfaces.Result
{

    public interface ICalculationResult : ISaveableResult
    {
        /// <summary>
        /// creation time of the result
        /// </summary>
        DateTime CreationTime { get; }
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



}
