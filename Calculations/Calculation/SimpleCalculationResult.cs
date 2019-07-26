using Interfaces.MeasuredData;
using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace Calculations.Calculation
{
    internal class SimpleCalculationResult : CalculationResult<double>, ISimpleCalculationResult
    {

        public SimpleCalculationResult(double result, DateTime creationTime, bool successful, IMeasurementSerie measurementSerie)
            : base(creationTime, successful, measurementSerie)
        {
            ResultValue = result;
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
