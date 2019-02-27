using Interfaces.Misc;
using PluginLoading.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.Repository
{
    class RepositoryFactory<T> : IPluginFactory
        where T : class, INamed, IComparable<T>
    {
        Dictionary<string, HDDRepository<T>> _dataCollectorDict = new Dictionary<string, HDDRepository<T>>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(Evaluation)))
            {
                if (!_dataCollectorDict.ContainsKey(name))
                {
                    HDDRepositoryParameters param = new HDDRepositoryParameters();
                    if (param.Load())
                    {
                        Evaluation instance = new HDDRE(param);
                        _dataCollectorDict.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _dataCollectorDict[name];
                }
            }
            return null;
        }

    }
}
