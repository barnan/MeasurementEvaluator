using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.ReferenceSample;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.ME_Repository
{
    class HDDReferenceRepository : HDDRepository<IReferenceSample>, IReferenceRepository
    {
        internal HDDReferenceRepository(HDDRepositoryParameters parameters)
        : base(parameters)
        {
            _repositoryPath = Frame.PluginLoader.PluginLoader.ReferenceFolder;
        }

    }

    public class HDDReferenceRepositoryFactory : IPluginFactory
    {
        private readonly Dictionary<string, IReferenceRepository> _specificationRepositoryDictionary = new Dictionary<string, IReferenceRepository>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDReferenceRepository)))
            {
                HDDRepositoryParameters parameters = new HDDRepositoryParameters();
                if (parameters.Load(name))
                {
                    HDDReferenceRepository instance = new HDDReferenceRepository(parameters);
                    _specificationRepositoryDictionary.Add(name, instance);
                }

                return _specificationRepositoryDictionary[name];
            }
            return null;
        }
    }

}
