using Interfaces.MeasuredData;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAcquisition.Repository
{
    public class HDDMeasurementDataRepository : HDDRepository<IToolMeasurementData>
    {

        public HDDMeasurementDataRepository(SpecificationRepositoryParameters parameters)
            : base(parameters)
        {
        }


        protected override List<IToolMeasurementData> GetItemList(string fullPath)
        {
            try
            {
                if (!CheckFolder(fullPath))
                {
                    _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
                    return null;
                }

                List<string> fileNameList = Directory.GetFiles(fullPath, $"*.{_parameters.FileExtensionFilter}").ToList();
                List<IToolMeasurementData> fileContentDictionary = new List<IToolMeasurementData>(fileNameList.Count);

                foreach (string fileName in fileNameList)
                {
                    IToolMeasurementData data = _parameters.HDDReaderWriter.ReadFile<IToolMeasurementData>(fileName);

                    fileContentDictionary.Add(data);

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
