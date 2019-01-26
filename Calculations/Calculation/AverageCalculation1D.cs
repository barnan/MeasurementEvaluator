using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.Result;
using Miscellaneous;
using System;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    class AverageCalculation1D : CalculationBase
    {

        public AverageCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.Average;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICalculationSettings settings)
        {
            DateTime startTime = _parameters.DateTimeProvider.GetDateTime();

            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);

            _parameters.Logger.MethodTrace($"{nameof(StdCalculation1D)}: Calculated average: {average}.");

            return new SimpleCalculationResult(average,
                                               startTime,
                                               _parameters.DateTimeProvider.GetDateTime(),
                                               true);
        }

    }
}
