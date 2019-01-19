using System;

namespace Interfaces.Misc
{
    public interface IInitializable
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Initiailze();

        /// <summary>
        /// 
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Initialized;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs> Closed;

    }
}
