using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.ME_Repository
{
    public class RepositoryFactory : IPluginFactory
    {
        Dictionary<string, IRepository> _repositoryDictionary = new Dictionary<string, IRepository>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDRepository)))
            {
                if (!_repositoryDictionary.ContainsKey(name))
                {
                    HDDRepositoryParameters parameters = new HDDRepositoryParameters();
                    if (parameters.Load(name))
                    {
                        HDDRepository instance = new HDDRepository(parameters);
                        _repositoryDictionary.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _repositoryDictionary[name];
                }
            }
            return null;
        }
    }
}
