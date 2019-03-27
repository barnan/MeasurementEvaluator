using Frame.PluginLoader.Interfaces;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation.CalculationContainer
{
    class Factory : IPluginFactory
    {

        Dictionary<string, CalculationContainer> _calculationContainerDict = new Dictionary<string, CalculationContainer>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(Evaluation.Evaluation)))
            {
                if (!_calculationContainerDict.ContainsKey(name))
                {
                    CalculationContainerParameters param = new CalculationContainerParameters();
                    if (param.Load(name))
                    {
                        CalculationContainer instance = new CalculationContainer(param);
                        _calculationContainerDict.Add(name, instance);
                    }
                }
                return _calculationContainerDict[name];
            }
            return null;
        }
    }
}
