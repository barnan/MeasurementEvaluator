using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.Result;
using Miscellaneous;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    internal class CpkCalculation1D : CalculationBase
    {
        public CpkCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.Cpk;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICalculationSettings settings)
        {
            DateTime startTime = _parameters.DateTimeProvider.GetDateTime();

            if (!(settings is ICpkCalculationSettings cpkSettings))
            {
                _parameters.Logger.LogError($"Arrived settings is null or it is not {nameof(ICpkCalculationSettings)}");
                return null;
            }

            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);

            double std = GetStandardDeviation(validElementList);

            double usl = cpkSettings.ReferenceValue + cpkSettings.HalfTolerance;

            double lsl = cpkSettings.ReferenceValue - cpkSettings.HalfTolerance;

            double cpk = Math.Min((average - usl) / (3 * std), (lsl - average) / (3 * std));

            _parameters.Logger.MethodTrace($"{nameof(StdCalculation1D)}: Calculated  Cp: {cpk}, USL: {usl}, LSL: {lsl}.");

            return new QCellsCalculationResult(cpk,
                                               usl,
                                               lsl,
                                               startTime,
                                               _parameters.DateTimeProvider.GetDateTime(),
                                               true);
        }
    }
}
