namespace Interfaces.DataAcquisition
{



    public interface IHDDFileReader
    {
        T ReadFromFile<T>(string fileNameAndPath, ToolNames toolName = null);

        bool CanRead(string fileNameAndPath);
    }




    public interface IHDDFileWriter
    {
        bool WriteToFile<T>(T obj, string fileNameAndPath);
    }



    public interface IHDDFileReaderWriter : IHDDFileReader, IHDDFileWriter
    {
    }



}
