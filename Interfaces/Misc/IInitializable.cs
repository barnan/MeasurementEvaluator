using System;

namespace Interfaces.Misc
{
    public interface IInitializable
    {

        /// <summary>
        /// The initialization comprises necessary settings and checks for the proper operation
        /// </summary>
        /// <returns></returns>
        bool Initiailze();

        /// <summary>
        /// property that show the current initialization state of the component. True -> the component is functinoal. False -> the component is not functional
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// It is contrary of the inititalization. It contains the memory deallocation or disposing is necessary.
        /// </summary>
        void Close();

        /// <summary>
        /// Initialized eventhandler is fired when the the component is initialized. 
        /// </summary>
        event EventHandler<EventArgs> Initialized;

        /// <summary>
        /// Closed eventhandler is fired when the the component is closed. 
        /// </summary>
        event EventHandler<EventArgs> Closed;

    }
}
