using Interfaces.ToolSpecifications;

namespace DataAcquisitions.Repository
{
    internal class HDDSpecificationRepository : HDDRepository<IToolSpecification>
    {

        public HDDSpecificationRepository(HDDRepositoryParameters parameters)
            : base(parameters)
        {
        }


        //protected override List<IToolSpecification> GetItemList(string fullPath)
        //{
        //    try
        //    {
        //        if (!CheckFolder(fullPath))
        //        {
        //            _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
        //            return null;
        //        }

        //        List<string> fileNameList = Directory.GetFiles(fullPath, $"*.{_parameters.FileExtensionFilters}").ToList();
        //        List<IToolSpecification> fileContentDictionary = new List<IToolSpecification>(fileNameList.Count);

        //        foreach (string fileName in fileNameList)
        //        {

        //            IToolSpecification spec = _parameters.IHDDReaderWriter.ReadFromFile<IToolSpecification>(fileName);

        //            fileContentDictionary.Add(spec);

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

    }

}
