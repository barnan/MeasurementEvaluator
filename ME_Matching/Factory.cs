using Frame.PluginLoader.Interfaces;
using System;
using System.Collections.Generic;

namespace MeasurementEvaluator.ME_Matching
{
    public class Factory : IPluginFactory
    {
        private readonly Dictionary<string, Matching> _matchingDict = new Dictionary<string, Matching>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(Matching)))
            {
                if (!_matchingDict.ContainsKey(name))
                {
                    MathchingParameters param = new MathchingParameters();
                    if (param.Load(name))
                    {
                        Matching instance = new Matching(param);
                        _matchingDict.Add(name, instance);
                    }
                }
                return _matchingDict[name];
            }
            return null;
        }
    }
}
