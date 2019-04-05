using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using System;
using System.Collections.Generic;

namespace MeasurementEvaluator.ME_DummyObjectCreator
{
    class Factory : IPluginFactory
    {
        readonly Dictionary<string, IDummyObjectCreator> _dummyObjectCretatorDict = new Dictionary<string, IDummyObjectCreator>();

        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(IDummyObjectCreator)))
            {
                if (!_dummyObjectCretatorDict.ContainsKey(name))
                {
                    IDummyObjectCreator instance = new DummyObjectCreator();
                    _dummyObjectCretatorDict.Add(name, instance);
                    return instance;
                }
                return _dummyObjectCretatorDict[name];
            }
            return null;
        }
    }
}
