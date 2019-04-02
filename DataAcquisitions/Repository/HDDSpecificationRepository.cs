using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.ToolSpecifications;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.Repository
{
    internal class HDDSpecificationRepository : HDDRepository<IToolSpecification>, IToolSpecificationRepository
    {

        internal HDDSpecificationRepository(HDDRepositoryParameters parameters)
        : base(parameters)
        {
            _repositoryPath = Frame.PluginLoader.PluginLoader.SpecificationFolder;
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
