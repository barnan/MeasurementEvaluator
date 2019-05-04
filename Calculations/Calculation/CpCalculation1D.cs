using Calculations.Calculation.CalculationSettings;
using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    internal class CpCalculation1D : CalculationBase, ICpCalculation
    {
        internal CpCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.Cp;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICalculationSettings settings)
        {
            DateTime startTime = _parameters.DateTimeProvider.GetDateTime();

            ICpCalculationSettings cpSettings = settings as ICpCalculationSettings;

            if (cpSettings == null)
            {
                throw new ArgumentNullException($"Arrived settings is null or it is not {nameof(ICpCalculationSettings)}");
            }

            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);
            double std = GetStandardDeviation(validElementList);
            double usl = average + cpSettings.HalfTolerance;
            double lsl = average - cpSettings.HalfTolerance;
            double cp = (usl - lsl) / (6 * std);

            _parameters.Logger.MethodTrace($"{nameof(StdCalculation1D)}: Calculated  Cp: {cp}, USL: {usl}, LSL: {lsl}.");

            return new QCellsCalculationResult(cp,
                                                usl,
                                                lsl,
                                                startTime,
                                                _parameters.DateTimeProvider.GetDateTime(),
                                                true);
        }


        public override ICalculationSettings CreateSettings(ICondition specification, IReferenceSample sample)
        {
            if (!(specification is ICpkCondition cpkCondition))
            {
                _parameters.Logger.Error($"Not valid condition received for settings creation.");
                return null;
            }

            return new CpCalculationSettings(CalculationType, cpkCondition.HalfTolerance);
        }

    }
}
