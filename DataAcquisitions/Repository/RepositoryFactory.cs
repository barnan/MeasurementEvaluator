using Interfaces.DataAcquisition;
using PluginLoading.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.Repository
{
    class RepositoryFactory<T> : IPluginFactory
        where T : class, IManagableFromRepository<T>
    {
        Dictionary<string, IRepository<T>> _repositoryDictionary = new Dictionary<string, IRepository<T>>();

        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDSpecificationRepository)) || t.IsAssignableFrom(typeof(HDDMeasurementDataRepository)) || t.IsAssignableFrom(typeof(HDDReferenceRepository)))
            {
                if (!_repositoryDictionary.ContainsKey(name))
                {
                    HDDRepositoryParameters parameters = new HDDRepositoryParameters();
                    if (parameters.Load())
                    {
                        if (t.IsAssignableFrom(typeof(HDDSpecificationRepository)))
                        {
                            HDDSpecificationRepository instance = new HDDSpecificationRepository(parameters);
                            _repositoryDictionary.Add(name, instance);
                        }

                        if (t.IsAssignableFrom(typeof(HDDReferenceRepository)))
                        {
                            HDDReferenceRepository instance = new HDDReferenceRepository(parameters);
                            _repositoryDictionary.Add(name, instance);
                        }

                        if (t.IsAssignableFrom(typeof(HDDMeasurementDataRepository)))
                        {
                            HDDMeasurementDataRepository instance = new HDDMeasurementDataRepository(parameters);
                            _repositoryDictionary.Add(name, instance);
                        }
                    }
                }
                return _repositoryDictionary[name];
            }
            return null;
        }

    }
}
