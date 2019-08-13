using Frame.PluginLoader.Interfaces;
using System;
using System.Collections.Generic;

namespace TabularDataProcessor
{
    public class Factory : IPluginFactory
    {
        private readonly Dictionary<string, TabularDataProcessor> _matchingDict = new Dictionary<string, TabularDataProcessor>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(TabularDataProcessor)))
            {
                if (!_matchingDict.ContainsKey(name))
                {
                    Parameters param = new Parameters();
                    TabularDataProcessor instance = new TabularDataProcessor(param);
                    //if (param.Load(name))
                    //{
                    //    _matchingDict.Add(name, instance);
                    //    return instance;
                    //}
                }
                else
                {
                    return _matchingDict[name];
                }
            }
            return null;
        }
    }
}
