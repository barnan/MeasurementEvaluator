using Interfaces.Misc;

namespace Interfaces.MeasurementEvaluator.Result
{
    public interface IResult : IXmlStorable
    {
        /// <summary>
        /// Contains the calculation end time
        /// </summary>
        DateTime CreationTime { get; }


        /// <summary>
        /// contains whether the calculation was successful or not. The calculation is successful, if all the sub-result were been able to calculate.
        /// </summary>
        bool IsSuccessful { get; }
    }


    public interface IResultProvider
    {
        ///// <summary>
        ///// Subscribe to ResultReadyEvent. ResultReadyEventHandler is fired when the calculation is finished
        ///// </summary>
        //void SubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method);


        ///// <summary>
        ///// Un-Subscribe to ResultReadyEvent. 
        ///// </summary>
        //void UnSubscribeToResultReadyEvent(EventHandler<ResultEventArgs> method);

        void FeedResult(IResult result);

        EventHandler<IResult> ResultReady { get; }
    }
}
