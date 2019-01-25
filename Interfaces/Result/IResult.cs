﻿using System;
using System.Xml.Linq;

namespace Interfaces.Result
{
    public interface IResult
    {
        /// <summary>
        /// Contains start time of the calculation 
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Contains the calculation end time
        /// </summary>
        DateTime EndTime { get; }


        /// <summary>
        /// contains whether the calculation was successful or not. The calculation is successful, if all the sub-result were been able to calculate.
        /// </summary>
        bool SuccessfulCalculation { get; }

    }


    public interface ISaveableResult : IResult
    {
        /// <summary>
        /// Save the result content into an XElement
        /// </summary>
        /// <returns></returns>
        XElement Save();

        /// <summary>
        /// Load the necessry data from an XElement
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool Load(XElement input);
    }



    public interface IResultProvider
    {
        /// <summary>
        /// ResultReadyEvent eventhandler is fired when the calculation is finished
        /// </summary>
        event EventHandler<ResultEventArgs> ResultReadyEvent;
    }


}