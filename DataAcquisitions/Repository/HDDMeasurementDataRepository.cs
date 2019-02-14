namespace DataAcquisitions.Repository
{
    //internal class HDDMeasurementDataRepository : HDDRepositoryBase<IToolMeasurementData>
    //{

    //    public HDDMeasurementDataRepository(HDDRepositoryParameters parameters)
    //        : base(parameters)
    //    {
    //    }


    //protected override List<IToolMeasurementData> GetItemList(string fullPath)
    //{
    //    try
    //    {
    //        if (!CheckFolder(fullPath))
    //        {
    //            _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
    //            return null;
    //        }

    //        List<string> fileNameList = Directory.GetFiles(fullPath, $"*.{_parameters.FileExtensionFilters}").ToList();
    //        List<IToolMeasurementData> fileContentDictionary = new List<IToolMeasurementData>(fileNameList.Count);

    //        foreach (string fileName in fileNameList)
    //        {
    //            IToolMeasurementData data = _parameters.IHDDReaderWriter.ReadFromFile<IToolMeasurementData>(fileName);

    //            fileContentDictionary.Add(data);

    //            if (_parameters.Logger.IsTraceEnabled)
    //            {
    //                _parameters.Logger.MethodTrace($"Specification file read: {fileName}");
    //            }
    //        }

    //        if (_parameters.Logger.IsTraceEnabled)
    //        {
    //            foreach (var item in fileContentDictionary)
    //            {
    //                _parameters.Logger.MethodTrace($"Specification: {item}");
    //            }
    //        }

    //        return fileContentDictionary;
    //    }
    //    catch (Exception ex)
    //    {
    //        _parameters.Logger.MethodError($"Exception occured: {ex}");
    //        return null;
    //    }
    //}


    //}
}
