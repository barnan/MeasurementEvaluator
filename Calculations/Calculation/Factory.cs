﻿using Frame.PluginLoader.Interfaces;
using Interfaces.BaseClasses;
using Interfaces.Calculation;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    class Factory : IPluginFactory
    {
        readonly Dictionary<string, ICalculation> _calculationDict = new Dictionary<string, ICalculation>();

        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(AverageCalculation1D)))
            {
                if (!_calculationDict.ContainsKey(name))
                {
                    CalculationParameters param = new CalculationParameters();
                    if (param.Load(name))
                    {
                        ICalculation instance;

                        switch (param.CalculationTypeValue)
                        {
                            case CalculationTypes.Average:
                                instance = new AverageCalculation1D(param);
                                break;
                            case CalculationTypes.StandardDeviation:
                                instance = new StdCalculation1D(param);
                                break;
                            case CalculationTypes.Cp:
                                instance = new CpCalculation1D(param);
                                break;
                            case CalculationTypes.Cpk:
                                instance = new CpkCalculation1D(param);
                                break;
                            case CalculationTypes.GRAndR:
                            case CalculationTypes.Unknown:
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        _calculationDict.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _calculationDict[name];
                }
            }

            return null;
        }
    }
}
