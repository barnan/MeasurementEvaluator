using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using Interfaces.BaseClasses;

namespace Calculations.Calculation
{
    class AverageCalculation1D : CalculationBase, ICalculation
    {

        internal AverageCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.Average;


        protected override IResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            if (condition.CalculationType != CalculationType)
            {
                throw new ArgumentException($"The current calculation (type: {CalculationType}) can not run with the received condition {condition.CalculationType}");
            }

            List<double> validElementList = GetValidElementList(measurementSerieData);
            double average = GetAverage(validElementList);

            _parameters.Logger.LogTrace($"{nameof(StdCalculation1D)}: Calculated average: {average}.");

            return new SimpleCalculationResult(average,
                                               _parameters.DateTimeProvider.GetDateTime(),
                                               true);
        }
    }
}
