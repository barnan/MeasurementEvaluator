using Frame.MessageHandler;
using Interfaces.DataAcquisition;
using NLog;

namespace MeasurementEvaluator.ME_Matching
{
    internal interface IPairingParameters
    {
        string BindingFilePath { get; }
        ILogger Logger { get; }
        IUIMessageControl MessageControl { get; }
        string Name { get; }
        IHDDFileReader PairingFileReader { get; }
    }
}