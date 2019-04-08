using NLog;

namespace DataAcquisitions.HDDTabularMeasurementFileReaderWriter
{
    internal class Parameters
    {
        internal char Separator { get; set; }
        internal ILogger Logger { get; set; }
    }
}
