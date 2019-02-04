using Interfaces.DataAcquisition;
using NLog;

namespace DataAcquisitions.Repository
{
    internal class HDDRepositoryParameters
    {
        internal string RepositoryFullDirectoryPath { get; }
        internal string FileExtensionFilter { get; }
        internal ILogger Logger { get; }
        internal IFileReaderWriter HDDReaderWriter { get; }


        internal HDDRepositoryParameters(string repostoryDirectoryPath, string fileExtensionFilter, IFileReaderWriter fileReaderWriter)
        {
            ILogger Logger = LogManager.GetCurrentClassLogger();
            RepositoryFullDirectoryPath = repostoryDirectoryPath;
            FileExtensionFilter = fileExtensionFilter;
            HDDReaderWriter = fileReaderWriter;
        }
    }



    internal class SpecificationRepositoryParameters : HDDRepositoryParameters
    {

        public SpecificationRepositoryParameters(string repostoryDirectoryPath, string fileExtensionFilter, IFileReaderWriter fileReaderWriter)
            : base(repostoryDirectoryPath, fileExtensionFilter, fileReaderWriter)
        {
        }

    }


}
