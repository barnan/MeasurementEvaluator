using System;
using System.Xml.Linq;

namespace Interfaces.Result
{
    public interface IResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        XElement Save();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool Load(XElement input);

        /// <summary>
        /// 
        /// </summary>
        DateTime CalculationStartTime { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime CalculationEndTime { get; }

    }


    public interface IResultProvider
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ResultEventArgs> ResultReadyEvent;
    }




}
