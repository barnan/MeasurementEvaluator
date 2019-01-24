using DataStructures.Calculation;
using Interfaces;
using Interfaces.Calculation;
using Interfaces.MeasuredData;
using Interfaces.Result;
using Miscellaneous;
using System.Collections.Generic;
using System.Linq;

namespace Calculations.Calculation
{
    class StdCalculation1D : CalculationBase
    {

        public StdCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.StandardDeviation;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData, ICalculationSettings settings)
        {
            List<double> validElementList = measurementSerieData.MeasData.Where(p => p.Valid).Select(p => p.Value).ToList();

            double std = GetStandardDeviation(validElementList);

            _parameters.Logger.MethodTrace($"{nameof(StdCalculation1D)}: Calculated standard devaition: {std}.");

            return new SimpleCalculationResult(_parameters.DateTimeProvider.GetDateTime(), std);
        }
    }
}
