using Interfaces.MeasuredData;
using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace Calculations.Calculation
{
    internal class QCellsCalculationResult : CalculationResult<double>, IQCellsCalculationResult
    {
        public double USL { get; }

        public double LSL { get; }


        public QCellsCalculationResult(double result, double usl, double lsl, DateTime creationTime, bool successful, IMeasurementSerie measurementSerie)
            : base(creationTime, successful, measurementSerie)
        {
            ResultValue = result;
            USL = usl;
            LSL = lsl;
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
