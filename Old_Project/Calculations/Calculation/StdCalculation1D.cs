using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using Interfaces.ReferenceSample;
using Interfaces.Result;
using Interfaces.ToolSpecifications;
using Miscellaneous;
using System.Collections.Generic;
using System.Linq;

namespace Calculations.Calculation
{
    class StdCalculation1D : CalculationBase
    {

        internal StdCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationType CalculationType => CalculationType.StandardDeviation;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICondition condition, IReferenceValue referenceValue)
        {
            List<double> validElementList = measurementSerieData.MeasuredPoints.Where(p => p.Valid).Select(p => p.Value).ToList();

            double average = GetAverage(validElementList);

            double std = GetStandardDeviation(validElementList, average);

            _parameters.Logger.LogTrace($"{nameof(StdCalculation1D)}: Calculated standard devaition: {std}, average: {average}");

            return new SimpleCalculationResult(_parameters.DateTimeProvider.GetDateTime(),
                                                true,
                                                measurementSerieData,
                                                std,
                                                average);
        }
    }
}
