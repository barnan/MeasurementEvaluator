using Frame.PluginLoader.Interfaces;
using System;
using System.Collections.Generic;

namespace MeasurementEvaluator.ME_Evaluation
{
    public class Factory : IPluginFactory
    {
        readonly Dictionary<string, Evaluation> _dataCollectorDict = new Dictionary<string, Evaluation>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(Evaluation)))
            {
                if (!_dataCollectorDict.ContainsKey(name))
                {
                    EvaluationParameters param = new EvaluationParameters();
                    if (param.Load(name))
                    {
                        Evaluation instance = new Evaluation(param);
                        _dataCollectorDict.Add(name, instance);
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
