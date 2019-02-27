using Interfaces.DataAcquisition;
using NLog;
using System.Collections.Generic;

namespace DataAcquisitions.Repository
{
    internal class HDDRepositoryParameters
    {
        internal string RepositoryFullDirectoryPath { get; set; }
        internal List<string> FileExtensionFilters { get; }
        internal ILogger Logger { get; }
        internal IHDDFileReaderWriter IHDDReaderWriter { get; }


        //internal HDDRepositoryParameters(string repostoryDirectoryPath, List<string> fileExtensionFilter, IHDDFileReaderWriter ihddFileReaderWriter)
        //{
        //    Logger = LogManager.GetCurrentClassLogger();
        //    RepositoryFullDirectoryPath = repostoryDirectoryPath;
        //    FileExtensionFilters = fileExtensionFilter;
        //    IHDDReaderWriter = ihddFileReaderWriter;
        //}
    }

}
