﻿


using Interfaces.Calculation;
using NLog;
using System.Collections.Generic;

namespace Calculations.Calculation.CalculationContainer
{
    internal class CalculationContainerParameters
    {
        private List<ICalculation> _availableCalculations = new List<ICalculation> { new AverageCalculation1D(new CalculationParameters()),
                                                                                     new StdCalculation1D(new CalculationParameters()) };
        public IReadOnlyList<ICalculation> AvailableCalculations => _availableCalculations;


        public ILogger Logger { get; internal set; }


        internal bool Load()
        {
            Logger = LogManager.GetCurrentClassLogger();

            return true;
        }

    }
}