using Frame.PluginLoader.Interfaces;
using Interfaces.Calculation;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    class Factory : IPluginFactory
    {

        Dictionary<string, ICalculation> _calculationDict = new Dictionary<string, ICalculation>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(CalculationBase)))
            {
                if (!_calculationDict.ContainsKey(name))
                {
                    CalculationParameters param = new CalculationParameters();
                    if (param.Load(name))
                    {
                        instance = new Evaluation.Evaluation(param);
                        _calculationDict.Add(name, instance);
                    }
                }
                return _calculationDict[name];
            }
            return null;
        }
    }
}
