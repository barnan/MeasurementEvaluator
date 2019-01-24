using Interfaces.Misc;
using Interfaces.Result;

namespace Interfaces.DataAcquisition
{
    public interface IDataCollector : IInitializable, IResultProvider
    {
        void Gather();
    }
}
