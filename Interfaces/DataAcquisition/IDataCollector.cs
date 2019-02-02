using Interfaces.Misc;
using Interfaces.Result;
using System;
using System.Collections.Generic;

namespace Interfaces.DataAcquisition
{
    public interface IDataCollector : IInitializable, IResultProvider
    {
        void GatherData(string specifactionName, List<string> measurementDataFileNames, string referenceName = null);

        void GatherNames();

        event EventHandler<DataCollectorResultEventArgs> FileNamesReadyEvent;

    }
}
