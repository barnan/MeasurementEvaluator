using Frame.PluginLoader.Interfaces;
using Interfaces;
using Interfaces.Calculation;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    class Factory : IPluginFactory
    {
        //readonly Dictionary<string, IAverageCalculation> _averageCalculationDict = new Dictionary<string, IAverageCalculation>();
        //readonly Dictionary<string, IStdCalculation> _stdCalculationDict = new Dictionary<string, IStdCalculation>();
        //readonly Dictionary<string, ICpCalculation> _cpkCalculationDict = new Dictionary<string, ICpCalculation>();
        //readonly Dictionary<string, ICpCalculation> _cpCalculationDict = new Dictionary<string, ICpCalculation>();

        readonly Dictionary<string, ICalculation> _calculationDict = new Dictionary<string, ICalculation>();

        public object Create(Type t, string name)
        {
            //if (t.IsAssignableFrom(typeof(AverageCalculation1D)))
            //{
            //    if (!_averageCalculationDict.ContainsKey(name))
            //    {
            //        CalculationParameters param = new CalculationParameters();
            //        if (param.Load(name))
            //        {
            //            var instance = new AverageCalculation1D(param);
            //            _averageCalculationDict.Add(name, instance);
            //            return instance;
            //        }
            //    }
            //    else
            //    {
            //        return _averageCalculationDict[name];
            //    }
            //}

            //if (t.IsAssignableFrom(typeof(StdCalculation1D)))
            //{
            //    if (!_stdCalculationDict.ContainsKey(name))
            //    {
            //        CalculationParameters param = new CalculationParameters();
            //        if (param.Load(name))
            //        {
            //            var instance = new StdCalculation1D(param);
            //            _stdCalculationDict.Add(name, instance);
            //            return instance;
            //        }
            //    }
            //    else
            //    {
            //        return _stdCalculationDict[name];
            //    }
            //}

            //if (t.IsAssignableFrom(typeof(CpkCalculation1D)))
            //{
            //    if (!_cpkCalculationDict.ContainsKey(name))
            //    {
            //        CalculationParameters param = new CalculationParameters();
            //        if (param.Load(name))
            //        {
            //            var instance = new CpkCalculation1D(param);
            //            _cpkCalculationDict.Add(name, instance);
            //            return instance;
            //        }
            //    }
            //    else
            //    {
            //        return _cpkCalculationDict[name];
            //    }
            //}

            //if (t.IsAssignableFrom(typeof(CpCalculation1D)))
            //{
            //    if (!_cpCalculationDict.ContainsKey(name))
            //    {
            //        CalculationParameters param = new CalculationParameters();
            //        if (param.Load(name))
            //        {
            //            var instance = new CpCalculation1D(param);
            //            _cpCalculationDict.Add(name, instance);
            //            return instance;
            //        }
            //    }
            //    else
            //    {
            //        return _cpCalculationDict[name];
            //    }
            //}


            if (t.IsAssignableFrom(typeof(AverageCalculation1D)))
            {
                if (!_calculationDict.ContainsKey(name))
                {
                    CalculationParameters param = new CalculationParameters();
                    if (param.Load(name))
                    {
                        ICalculation instance = null;

                        switch (param.CalculationType)
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
