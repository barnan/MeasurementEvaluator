using Interfaces.Misc;
using Interfaces.Result;
using System.Collections.Generic;

namespace Interfaces.DataAcquisition
{
    public interface IDataCollector : IInitializable, IResultProvider
    {
        void Gather(string specifactionName, List<string> measurementDataFileNames, string referenceName = null);

        IReadOnlyList<string> GetAllSpecificationNames();

        IReadOnlyList<string> GetAllMeasurementFileNames();

        IReadOnlyList<string> GetAllRferenceSampleNames();
    }
}
