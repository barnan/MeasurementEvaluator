using Interfaces.MeasuredData;

namespace DataAcquisition.Repository
{
    class HDDMeasurementDataRepository : HDDRepository<IToolMeasurementData>
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
                List<IToolSpecification> fileContentDictionary = new List<IToolSpecification>(fileNameList.Count);
                //List<XmlDocument> documents = new List<XmlDocument>();

                foreach (string fileName in fileNameList)
                {
                    ToolSpecificationOnHDD specOnHDD = new ToolSpecificationOnHDD();

                    XmlDocument currentXmlDocument = new XmlDocument();
                    currentXmlDocument.LoadXml(fileName);

                    _parameters.XmlParser.ParseDocument(specOnHDD, currentXmlDocument);

                    fileContentDictionary.Add(new ToolSpecification(fileName, specOnHDD));

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
