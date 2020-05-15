using Interfaces.MeasuredData;
using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace Calculations.Calculation
{
    internal class SimpleCalculationResult : CalculationResult<double>, ISimpleCalculationResult
    {
        public SimpleCalculationResult(DateTime creationTime, bool successful, IMeasurementSerie measurementSerie, double result, double average)
            : base(creationTime, successful, measurementSerie, result, average)
        {
        }

        public override XElement SaveToXml(XElement input)
        {
            throw new NotImplementedException();
        }

        public override bool LoadFromXml(XElement input)
        {
            throw new NotImplementedException();
        }
    }
}
