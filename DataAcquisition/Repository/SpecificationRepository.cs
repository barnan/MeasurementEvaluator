using DataStructures.ToolSpecifications;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;

namespace DataAcquisition.Repository
{
    class SpecificationRepository : HDDRepository<IToolSpecification>
    {

        public SpecificationRepository(SpecificationRepositoryParameter parameters)
            : base(parameters)
        {
        }

        #region IRepository<T>

        public override IEnumerable<IToolSpecification> GetAll()
        {
            return GetItemList(_parameters.RepositoryFullDirectoryPath);
        }


        public override IEnumerable<IToolSpecification> Find(Expression<Func<IToolSpecification>> predicate)
        {
            throw new NotImplementedException();
        }





        #endregion


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





    public class SpecificationRepositoryParameter : SimpleHDDRepositoryParameter
    {
    }

}
