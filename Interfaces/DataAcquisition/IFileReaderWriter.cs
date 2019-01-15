namespace Interfaces.DataAcquisition
{

    public interface IFileReader
    {
        T ReadFile<T>(string fileNameAndPath, string toolName = null);

        bool CanRead(string fileNameAndPath);
    }


    public interface IFileWriter
    {
        bool WriteFile<T>(T obj, string fileNameAndPath);
    }



    public interface IFileReaderWriter : IFileReader, IFileWriter
    {
    }


}
