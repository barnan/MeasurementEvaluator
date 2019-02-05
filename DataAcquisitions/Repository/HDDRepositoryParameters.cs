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
        internal IFileReaderWriter HDDReaderWriter { get; }


        internal HDDRepositoryParameters(string repostoryDirectoryPath, List<string> fileExtensionFilter, IFileReaderWriter fileReaderWriter)
        {
            ILogger Logger = LogManager.GetCurrentClassLogger();
            RepositoryFullDirectoryPath = repostoryDirectoryPath;
            FileExtensionFilters = fileExtensionFilter;
            HDDReaderWriter = fileReaderWriter;
        }
    }



    internal class SpecificationRepositoryParameters : HDDRepositoryParameters
    {

        public SpecificationRepositoryParameters(string repostoryDirectoryPath, List<string> fileExtensionFilter, IFileReaderWriter fileReaderWriter)
            : base(repostoryDirectoryPath, fileExtensionFilter, fileReaderWriter)
        {
        }

    }


}
