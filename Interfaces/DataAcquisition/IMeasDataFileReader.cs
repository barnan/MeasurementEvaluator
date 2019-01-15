using Interfaces.MeasuredData;

namespace Interfaces.DataAcquisition
{
    public interface IMeasDataFile
    {
        IToolMeasurementData ReadFile(string fileNameAndPath, string toolName);
        bool CanRead(string fileNameAndPath);
    }
}
