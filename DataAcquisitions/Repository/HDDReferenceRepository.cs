using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.ReferenceSample;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.Repository
{
    class HDDReferenceRepository : HDDTypedRepository<IReferenceSample>
    {
        internal HDDReferenceRepository(HDDRepositoryParameters parameters)
        : base(parameters)
        {
        }

    }

    public class HDDReferenceRepositoryFactory : IPluginFactory
    {
        private readonly Dictionary<string, IRepository<IReferenceSample>> _specificationRepositoryDictionary = new Dictionary<string, IRepository<IReferenceSample>>();


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
