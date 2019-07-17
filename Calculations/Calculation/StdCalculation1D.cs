using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculations.Calculation
{
    class StdCalculation1D : CalculationBase, ICalculation
    {

        internal StdCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.StandardDeviation;


        protected override IResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            if (condition.CalculationType != CalculationType)
            {
                throw new ArgumentException($"The current calculation (type: {CalculationType}) can not run with the received condition {condition.CalculationType}");
            }

            List<double> validElementList = measurementSerieData.MeasuredPoints.Where(p => p.Valid).Select(p => p.Value).ToList();
            double std = GetStandardDeviation(validElementList);

            _parameters.Logger.LogTrace($"{nameof(StdCalculation1D)}: Calculated standard devaition: {std}.");

            return new SimpleCalculationResult(std,
                                               _parameters.DateTimeProvider.GetDateTime(),
                                               true);
        }
    }
}
