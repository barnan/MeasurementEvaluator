using PluginLoading.Interfaces;
using System;
using System.Collections.Generic;

namespace Calculations.Evaluation
{
    class Factory : IPluginFactory
    {

        Dictionary<string, Evaluation> _dataCollectorDict = new Dictionary<string, Evaluation>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(Evaluation)))
            {
                if (!_dataCollectorDict.ContainsKey(name))
                {
                    EvaluationParameters param = new EvaluationParameters();
                    if (param.Load())
                    {
                        Evaluation instance = new Evaluation(param);
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
