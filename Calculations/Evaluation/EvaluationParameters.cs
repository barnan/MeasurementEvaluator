﻿using Interfaces.Calculation;
using NLog;

namespace Calculations.Evaluation
{
    class EvaluationParameters
    {
        public ILogger Logger { get; private set; }
        public ICalculationContainer CalculationContainer { get; }


        public EvaluationParameters()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

    }
}