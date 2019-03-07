﻿using PluginLoading.Interfaces;
using System;
using System.Collections.Generic;

namespace Calculations.Matching
{
    class Factory : IPluginFactory
    {
        Dictionary<string, Matching> _matchingDict = new Dictionary<string, Matching>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(Matching)))
            {
                if (!_matchingDict.ContainsKey(name))
                {
                    MathchingParameters param = new MathchingParameters();
                    if (param.Load())
                    {
                        Matching instance = new Matching(param);
                        _matchingDict.Add(name, instance);
                        return instance;
                    }
                }
                else
                {
                    return _matchingDict[name];
                }
            }

            return null;
        }
    }
}
