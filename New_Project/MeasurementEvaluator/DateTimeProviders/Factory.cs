﻿using FrameInterfaces;
using Interfaces.Misc;

namespace DateTimeProviders
{
    public class Factory : IPluginFactory
    {
        readonly Dictionary<string, IDateTimeProvider> _fakeDateTimeProviderDict = new Dictionary<string, IDateTimeProvider>();
        readonly Dictionary<string, IDateTimeProvider> _simulatedDateTimeProviderDict = new Dictionary<string, IDateTimeProvider>();
        readonly Dictionary<string, IDateTimeProvider> _simpleDateTimeProviderDict = new Dictionary<string, IDateTimeProvider>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(IDateTimeProvider)) && name.Contains("Fake"))
            {
                if (!_fakeDateTimeProviderDict.ContainsKey(name))
                {
                    FakeDateTimeProviderParameter param = new FakeDateTimeProviderParameter();
                    if (param.Load(name))
                    {
                        IDateTimeProvider instance = new FakeDateTimeProvider(param);
                        _fakeDateTimeProviderDict.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _fakeDateTimeProviderDict[name];
                }
            }

            if (t.IsAssignableFrom(typeof(IDateTimeProvider)) && name.Contains("Simulated"))
            {
                if (!_simulatedDateTimeProviderDict.ContainsKey(name))
                {
                    SimulatedDateTimeProviderParameter param = new SimulatedDateTimeProviderParameter();
                    if (param.Load(name))
                    {
                        IDateTimeProvider instance = new SimulatedDateTimeProvider(param);
                        _simulatedDateTimeProviderDict.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _simulatedDateTimeProviderDict[name];
                }
            }

            if (t.IsAssignableFrom(typeof(IDateTimeProvider)))
            {
                if (!_simpleDateTimeProviderDict.ContainsKey(name))
                {
                    IDateTimeProvider instance = new SimpleDateTimeProvider(name);
                    _simpleDateTimeProviderDict.Add(name, instance);
                    return instance;
                }

                return _simpleDateTimeProviderDict[name];
            }

            return null;
        }
    }
}
