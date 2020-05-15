using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using System;
using System.Collections.Generic;

namespace DateTimeProviders
{
    public class Factory : IPluginFactory
    {
        readonly Dictionary<string, IDateTimeProvider> _fakeDateTimeProviderDict = new Dictionary<string, IDateTimeProvider>();
        readonly Dictionary<string, IDateTimeProvider> _simulatedDateTimeProviderDict = new Dictionary<string, IDateTimeProvider>();
        readonly Dictionary<string, IDateTimeProvider> _standardDateTimeProviderDict = new Dictionary<string, IDateTimeProvider>();


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
                if (!_standardDateTimeProviderDict.ContainsKey(name))
                {
                    IDateTimeProvider instance = new StandardDateTimeProvider(name);
                    _standardDateTimeProviderDict.Add(name, instance);
                    return instance;
                }
                else
                {
                    return _standardDateTimeProviderDict[name];
                }
            }

            return null;
        }
    }
}
