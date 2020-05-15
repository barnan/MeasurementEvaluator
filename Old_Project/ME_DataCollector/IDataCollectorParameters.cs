using Frame.MessageHandler;
using Interfaces.DataAcquisition;
using Interfaces.Misc;
using NLog;

namespace ME_DataCollector
{
    internal interface IDataCollectorParameters
    {
        ILogger Logger { get; }
        IDateTimeProvider DateTimeProvider { get; }
        IRepository MeasurementDataRepository { get; }
        IRepository ReferenceRepository { get; }
        IRepository SpecificationRepository { get; }
        IUIMessageControl MessageControl { get; }
        string Name { get; }
    }
}
