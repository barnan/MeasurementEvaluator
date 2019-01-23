using DataStructures.Calculation;
using Interfaces;
using Interfaces.MeasuredData;
using Interfaces.Result;
using System;
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


        protected override ICalculationResult InternalCalculation(IMeasurementSerie measurementSerieData)
        {
            if (measurementSerieData?.MeasData == null)
            {
                return null;
            }

            List<double> validElementList = measurementSerieData.MeasData.Where(p => p.Valid).Select(p => p.Value).ToList();

            double average = validElementList.Average();

            double sumOfDerivation = 0;

            foreach (double value in validElementList)
            {
                sumOfDerivation += (value) * (value);
            }
            double sumOfDerivationAverage = sumOfDerivation / (validElementList.Count - 1);

            double std = Math.Sqrt(sumOfDerivationAverage - (average * average));

            return new SimpleCalculationResult(DateTime.Now, std);
        }
    }
}
