using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using System;
using System.Collections.Generic;

namespace Miscellaneous
{
    internal sealed class StandardDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public StandardDateTimeProvider()
        {
        }
    }


    internal sealed class SimulatedDateTimeProvider : IDateTimeProvider
    {
        SimulatedDateTimeProviderParameter _parameter;


        public DateTime GetDateTime()
        {
            return _parameter.DateTimeToUse;
        }


        public SimulatedDateTimeProvider(SimulatedDateTimeProviderParameter parameter)
        {
            _parameter = parameter;
        }
    }


    internal sealed class SimulatedDateTimeProviderParameter
    {
        public DateTime DateTimeToUse { get; }
    }




    public class Factory : IPluginFactory
    {

        private Dictionary<string, IDateTimeProvider> _dateTimeProviderDictionary = new Dictionary<string, IDateTimeProvider>();

        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(StandardDateTimeProvider)))
            {
                if (!_dateTimeProviderDictionary.ContainsKey(name))
                {
                    //EvaluationParameters param = new EvaluationParameters();
                    //if (param.Load(name))
                    {
                        StandardDateTimeProvider instance = new StandardDateTimeProvider();
                        _dateTimeProviderDictionary.Add(name, instance);
                    }
                }
                return _dateTimeProviderDictionary[name];
            }
            return null;
        }
    }






}
