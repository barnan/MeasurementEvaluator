﻿using FrameInterfaces;
using Interfaces.Misc;

namespace ME_DummyObjectCreator
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
                    DummyObjectCreatorParameters param = new DummyObjectCreatorParameters();
                    if (param.Load(name))
                    {
                        IDummyObjectCreator instance = new DummyObjectCreator(param);
                        _dummyObjectCretatorDict.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _dummyObjectCretatorDict[name];
                }
            }
            return null;
        }
    }
}