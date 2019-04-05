using NLog;

namespace DataAcquisitions
{
    internal class TabularTextReaderParameters
    {
        internal char Separator { get; set; }
        internal ILogger Logger { get; set; }
    }
}
