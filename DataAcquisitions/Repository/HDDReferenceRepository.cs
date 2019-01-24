using Interfaces.ReferenceSample;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAcquisition.Repository
{
    class HDDReferenceRepository : HDDRepository<IReferenceSample>
    {
        public HDDReferenceRepository(HDDRepositoryParameters parameters)
            : base(parameters)
        {
        }


        protected override List<IReferenceSample> GetItemList(string fullPath)
        {
            try
            {
                if (!CheckFolder(fullPath))
                {
                    _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
                    return null;
                }

                List<string> fileNameList = Directory.GetFiles(fullPath, $"*.{_parameters.FileExtensionFilter}").ToList();
                List<IReferenceSample> fileContentDictionary = new List<IReferenceSample>(fileNameList.Count);

                foreach (string fileName in fileNameList)
                {
                    IReferenceSample refSampl = _parameters.HDDReaderWriter.ReadFile<IReferenceSample>(fileName);

                    fileContentDictionary.Add(refSampl);

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
}
