using Frame.PluginLoader.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.DataCollector
{
    public class Factory : IPluginFactory
    {

        private Dictionary<string, DataCollector> _dataCollectorDict = new Dictionary<string, DataCollector>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(DataCollector)))
            {
                if (!_dataCollectorDict.ContainsKey(name))
                {
                    DataCollectorParameters param = new DataCollectorParameters();

                    if (param.Load(name))
                    {
                        DataCollector instance = new DataCollector(param);
                        _dataCollectorDict.Add(name, instance);
                    }
                }
                return _dataCollectorDict[name];
            }
            return null;
        }

    }
}
