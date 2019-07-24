using Interfaces.BaseClasses;
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
        /// <param name="specifaction">specification</param>
        /// <param name="measurementDataFileNames">measurement data files</param>
        /// <param name="reference">reference. This finput parameter can be empty, because not all evaluations require reference samples (or the reference can be unknown)</param>
        void GatherData(IToolSpecification specifaction, IEnumerable<string> measurementDataFileNames, IReferenceSample reference = null);

        IEnumerable<IToolSpecification> GetAvailableToolSpecifications();

        IEnumerable<IToolSpecification> GetSpecificationsByToolName(ToolNames toolName);

        IEnumerable<IReferenceSample> GetReferenceSamples();

        string MeasurementFolderPath { get; }
    }
}
