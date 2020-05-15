using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System.Collections.Generic;

namespace Calculations.Calculation
{
    class AverageCalculation1D : CalculationBase
    {

        internal AverageCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationType CalculationType => CalculationType.Average;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            List<double> validElementList = GetValidElementList(measurementSerieData);
            double average = GetAverage(validElementList);

            _parameters.Logger.LogTrace($"{nameof(AverageCalculation1D)}: Calculated average: {average}.");

            return new SimpleCalculationResult(_parameters.DateTimeProvider.GetDateTime(),
                                               true,
                                               measurementSerieData,
                                               average,
                                               average);
        }
    }
}
