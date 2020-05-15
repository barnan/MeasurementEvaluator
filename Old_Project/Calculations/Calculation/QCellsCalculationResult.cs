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


        public QCellsCalculationResult(DateTime creationTime, bool successful, IMeasurementSerie measurementSerie, double result, double usl, double lsl, double average)
            : base(creationTime, successful, measurementSerie, result, average)
        {
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
