namespace Interfaces.DataAcquisition
{



    public interface IFileReader
    {
        T ReadFromFile<T>(string fileNameAndPath, ToolNames toolName = null);

        bool CanRead(string fileNameAndPath);
    }




    public interface IFileWriter
    {
        bool WriteToFile<T>(T obj, string fileNameAndPath);
    }



    public interface IFileReaderWriter : IFileReader, IFileWriter
    {
    }



}
