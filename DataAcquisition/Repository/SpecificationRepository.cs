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
            return GetItemList(_parameters.FullDirectoryPath);
        }

        // TODO: finish, compare the content of the file and delete the file if equal
        public override void Remove(IToolSpecification item)
        {
            try
            {
                if (item?.FileFullPathAndName == null)
                {
                    _parameters.Logger.MethodError("Arrived specification or its filename is null.");
                    return;
                }

                File.Delete(item.FileFullPathAndName);
            }
            catch (Exception ex)
            {
                _parameters.Logger.MethodError($"Delete of {item} was not successful: {ex}");
            }
        }


        //public override void RemoveRange(IEnumerable<IToolSpecification> items)
        //{
        //    foreach (var item in items)
        //    {
        //        Remove(item);
        //    }
        //}

        public override IEnumerable<IToolSpecification> Find(Expression<Func<IToolSpecification>> predicate)
        {
            throw new NotImplementedException();
        }


        public override IToolSpecification Get(int index, IComparer<IToolSpecification> comparer = null)
        {
            try
            {
                if (index < 0)
                {
                    _parameters.Logger.MethodError("The arrived index is below 0..");
                    return null;
                }

                List<IToolSpecification> specificationList = GetItemList(_parameters.FullDirectoryPath);

                if (index > specificationList.Count)
                {
                    _parameters.Logger.MethodError("The arrived index is higher than the length of the specification list.");
                    return null;
                }

                specificationList.Sort(comparer);

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


        protected override List<IToolSpecification> GetItemList(string fullPath)
        {
            try
            {
                if (!CheckFolder(fullPath))
                {
                    _parameters.Logger.MethodError($"The given folder can not be used: {fullPath}");
                    return null;
                }

                List<string> fileNameList = Directory.GetFiles(fullPath).ToList();
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
