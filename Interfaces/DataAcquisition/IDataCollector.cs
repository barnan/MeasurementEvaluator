using Interfaces.Misc;
using Interfaces.Result;
using System;
using System.Collections.Generic;

namespace Interfaces.DataAcquisition
{
    public interface IDataCollector : IInitializable, IResultProvider
    {
        /// <summary>
        /// Gatheres the data with the given name inputs. When ready calls its ResultReady event.
        /// </summary>
        /// <param name="specifactionName">name of the required specification</param>
        /// <param name="measurementDataFileNames">names of the required measurement data files</param>
        /// <param name="referenceName">names of required reference. This finput parameter can be empty, because not all evaluations require reference samples (or the reference can be unknown)</param>
        void GatherData(string specifactionName, List<string> measurementDataFileNames, string referenceName = null);

        /// <summary>
        /// Method the gather the available measurement data, speficiation and reference file names from the given media
        /// </summary>
        void GatherNames();

        /// <summary>
        /// Fires an event when the GatherNames is finished
        /// </summary>
        event EventHandler<DataCollectorResultEventArgs> FileNamesReadyEvent;

    }
}
