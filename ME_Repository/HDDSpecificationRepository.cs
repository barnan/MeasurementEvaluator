using DataStructures.ToolSpecifications;
using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DataAcquisitions.ME_Repository
{
    internal class HDDSpecificationRepository : HDDRepository<IToolSpecification>, IToolSpecificationRepository
    {

        internal HDDSpecificationRepository(HDDRepositoryParameters parameters)
        : base(parameters)
        {
            _repositoryPath = Frame.PluginLoader.PluginLoader.SpecificationFolder;
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

                List<string> fileNameList = Directory.GetFiles(fullPath, $"*.{_parameters.FileExtensionFilters}").ToList();
                List<IToolSpecification> fileContentDictionary = new List<IToolSpecification>(fileNameList.Count);

                foreach (string fileName in fileNameList)
                {
                    XElement inputelement = XElement.Load(fullPath);
                    IToolSpecification spec = new ToolSpecification();
                    spec.LoadFromXml(inputelement);

                    fileContentDictionary.Add(spec);

                    if (_parameters.Logger.IsTraceEnabled)
                    {
                        _parameters.Logger.MethodTrace($"File read: {fileName}");
                    }
                }

                if (_parameters.Logger.IsTraceEnabled)
                {
                    foreach (var item in fileContentDictionary)
                    {
                        _parameters.Logger.MethodTrace($"Items: {item}");
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


    public class HDDSpecificationRepositoryFactory : IPluginFactory
    {
        private readonly Dictionary<string, IToolSpecificationRepository> _specificationRepositoryDictionary = new Dictionary<string, IToolSpecificationRepository>();

        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDSpecificationRepository)))
            {
                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
                if (parameters.Load(name))
                {
                    HDDSpecificationRepository instance = new HDDSpecificationRepository(parameters);
                    _specificationRepositoryDictionary.Add(name, instance);
                }

                return _specificationRepositoryDictionary[name];
            }
            return null;
        }
    }

}
