using DataStructures.Calculation;
using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.Result;
using Miscellaneous;
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
            List<double> validElementList = GetValidElementList(measurementSerieData);

            double average = GetAverage(validElementList);

            _parameters.Logger.MethodTrace($"{nameof(StdCalculation1D)}: Calculated average: {average}.");

            return new SimpleCalculationResult(_parameters.DateTimeProvider.GetDateTime(), average);
        }

    }
}
