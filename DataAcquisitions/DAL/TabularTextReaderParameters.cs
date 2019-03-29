using NLog;

namespace DataAcquisitions.DAL
{
    internal class TabularTextReaderParameters
    {
        internal char Separator { get; set; }
        internal ILogger Logger { get; set; }
    }
}
