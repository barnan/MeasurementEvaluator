using Interfaces.DataAcquisition;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAcquisition.Repository
{
    internal class HDDSpecificationRepository : HDDRepository<IToolSpecification>
    {

        public HDDSpecificationRepository(SpecificationRepositoryParameters parameters)
            : base(parameters)
        {
        }


        protected override List<IToolSpecification> GetItemList(string fullPath)
        {
            try
            {
                if (!CheckFolder(fullPath))
                {
                    _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
                    return null;
                }

                List<string> fileNameList = Directory.GetFiles(fullPath, $"*.{_parameters.FileExtensionFilter}").ToList();
                List<IToolSpecification> fileContentDictionary = new List<IToolSpecification>(fileNameList.Count);

                foreach (string fileName in fileNameList)
                {

                    IToolSpecification spec = _parameters.HDDReaderWriter.ReadFile<IToolSpecification>(fileName);

                    fileContentDictionary.Add(spec);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace($"Specification file read: {fileName}");
                    }
                }

                if (_parameters.Logger.IsTraceEnabled)
                {
                    foreach (var item in fileContentDictionary)
                    {
                        _parameters.Logger.MethodTrace($"Specification: {item}");
                    }
                }

                return fileContentDictionary;
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Exception occured: {ex}");
                return null;
            }
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
