using DataStructures.Calculation;
using Interfaces;
using Interfaces.MeasuredData;
using Interfaces.Result;
using System;
using System.Linq;

namespace Calculations.Calculation
{
    class AverageCalculation1D : CalculationBase
    {

        public AverageCalculation1D(CalculationParameters parameters)
            : base(parameters)
        {
        }

        public override CalculationTypes CalculationType => CalculationTypes.Average;


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData)
        {
            if (measurementSerieData?.MeasData == null)
            {
                return null;
            }

            double average = measurementSerieData.MeasData.Where(p => p.Valid).Select(p => p.Value).Average();

            return new SimpleCalculationResult(DateTime.Now, average);
        }
    }
}
