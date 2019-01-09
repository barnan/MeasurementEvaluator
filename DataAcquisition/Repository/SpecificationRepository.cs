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
    class SpecificationRepository : SimpleHDDRepository<IToolSpecification>
    {
        private Dictionary<string, IToolSpecification> _fileContentDictionary;


        public SpecificationRepository(SimpleHDDRepositoryParameter parameters)
            : base(parameters)
        {
        }

        #region IRepository<T>

        public override IEnumerable<IToolSpecification> GetAll()
        {
            try
            {
                List<IToolSpecification> specificationList = GetSpecificationList(_parameters.FullDirectoryPath);
                return specificationList;
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Exception occured: {ex}");
                return null;
            }
        }

        // TODO: finish, compare the content of the file and delete the file if equals
        public override void Remove(IToolSpecification item)
        {
            try
            {
                List<IToolSpecification> specificationList = GetSpecificationList(_parameters.FullDirectoryPath);

                specificationList.Remove(item);
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Exception occured: {ex}");
                return;
            }
        }


        public override void RemoveRange(IEnumerable<IToolSpecification> items)
        {

        }

        public override IEnumerable<IToolSpecification> Find(Expression<Func<IToolSpecification>> predicate)
        {
            throw new NotImplementedException();
        }


        public override IToolSpecification Get(int index, IComparer<IToolSpecification> comprarer = null)
        {
            try
            {
                if (index < 0)
                {
                    _parameters.Logger.MethodError("The arrived index is below 0..");
                    return null;
                }

                List<IToolSpecification> specificationList = GetSpecificationList(_parameters.FullDirectoryPath);

                specificationList.Sort();

                if (index > specificationList.Count)
                {
                    _parameters.Logger.MethodError("The arrived index is higher than the length of the specification list.");
                    return null;
                }

                return specificationList[index];

            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Exception occured: {ex}");
                return null;
            }
        }


        public override void Add(IToolSpecification item)
        {

        }


        public override void AddRange(IEnumerable<IToolSpecification> items)
        {
            throw new NotImplementedException();
        }


        #endregion


        private List<IToolSpecification> GetSpecificationList(string fullPath)
        {
            if (!CheckFolder(fullPath))
            {
                _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
                return null;
            }

            _fileContentDictionary = new Dictionary<string, IToolSpecification>();

            List<string> fileNameList = Directory.GetFiles(fullPath).ToList();
            List<IToolSpecification> specificationList = new List<IToolSpecification>(fileNameList.Count);
            List<XmlDocument> documents = new List<XmlDocument>();

            foreach (string fileName in fileNameList)
            {
                IToolSpecification spec = new ToolSpecification();

                XmlDocument currentXmlDocument = new XmlDocument();
                currentXmlDocument.LoadXml(fileName);

                _parameters.XmlParser.ParseDocument(spec, currentXmlDocument);

                specificationList.Add(spec);
                _fileContentDictionary.Add(fileName, spec);


                if (_parameters.Logger.IsTraceEnabled)
                {
                    _parameters.Logger.MethodTrace($"Specification file read: {fileName}");
                }
            }

            if (_parameters.Logger.IsTraceEnabled)
            {
                foreach (var item in _fileContentDictionary)
                {
                    _parameters.Logger.MethodTrace($"FileName: {item.Key}, Content: {item.Value}");
                }
            }

            return specificationList;
        }



    }
}
