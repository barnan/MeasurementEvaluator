using System;

namespace Interfaces.DataAcquisition
{

    public interface IHDDFileReader
    {
        object ReadFromFile(string fileNameAndPath, Type type = null, ToolNames toolName = null);

        bool CanRead(string fileNameAndPath);
    }


    public interface IHDDFileWriter
    {
        bool WriteToFile(object obj, string fileNameAndPath);
    }


    public interface IHDDFileReaderWriter : IHDDFileReader, IHDDFileWriter
    {
    }



}
