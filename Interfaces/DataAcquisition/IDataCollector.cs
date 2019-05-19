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
        /// <param name="specifaction">name of the required specification</param>
        /// <param name="measurementDataFileNames">names of the required measurement data files</param>
        /// <param name="reference">names of required reference. This finput parameter can be empty, because not all evaluations require reference samples (or the reference can be unknown)</param>
        void GatherData(IToolSpecification specifaction, List<string> measurementDataFileNames, IReferenceSample reference = null);

        List<ToolNames> GetAvailableToolNames();

        List<IToolSpecification> GetSpecifications(ToolNames toolName);

        List<IReferenceSample> GetReferenceSamples();
    }
}
