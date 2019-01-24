﻿using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.Result;
using Miscellaneous;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    internal class CpCalculation1D : CalculationBase
    {
        public CpCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }



        public override CalculationTypes CalculationType => CalculationTypes.Cp;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICalculationSettings settings)
        {
            ICpCalculationSettings cpSettings = settings as ICpCalculationSettings;

            if (cpSettings == null)
            {
                _parameters.Logger.LogError($"Arrived settings is null or it is not {nameof(ICpCalculationSettings)}");
                return null;
            }

            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);

            double std = GetStandardDeviation(validElementList);

            double usl = average + cpSettings.HalfTolerance;

            double lsl = average - cpSettings.HalfTolerance;

            double cp = (usl - lsl) / (6 * std);

            _parameters.Logger.MethodTrace($"{nameof(StdCalculation1D)}: Calculated  Cp: {cp}, USL: {usl}, LSL: {lsl}.");

            return new QCellsCalculationResult(_parameters.DateTimeProvider.GetDateTime(), cp, usl, lsl);
        }



    }
}
