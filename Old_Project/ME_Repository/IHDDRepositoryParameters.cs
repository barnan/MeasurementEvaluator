using Frame.MessageHandler;
using Interfaces.DataAcquisition;
using NLog;
using System.Collections.Generic;

namespace DataAcquisitions.ME_Repository
{
    internal interface IHDDRepositoryParameters
    {
        List<string> FileExtensionFilters { get; }
        IHDDFileReaderWriter HDDReaderWriter { get; }
        ILogger Logger { get; }
        IUIMessageControl MessageControl { get; }
        string Name { get; }
    }
}