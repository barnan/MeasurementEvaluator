using Interfaces.Misc;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
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

        List<ToolNames> GetAvailableToolNames();

        List<IToolSpecification> GetSpecifications(ToolNames toolName);

        List<IReferenceSample> GetReferenceSamples();
    }
}
