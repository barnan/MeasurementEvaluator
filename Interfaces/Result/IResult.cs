using Interfaces.Misc;
using System;

namespace Interfaces.Result
{
    public interface IResult : IXmlStorable
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
        bool Successful { get; }
    }


    public interface IResultProvider
    {
        /// <summary>
        /// Subscribe to ResultReadyEvent. ResultReadyEventHandler is fired when the calculation is finished
        /// </summary>
        void SubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method);


        /// <summary>
        /// Un-Subscribe to ResultReadyEvent. 
        /// </summary>
        void UnSubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method);
    }

}
