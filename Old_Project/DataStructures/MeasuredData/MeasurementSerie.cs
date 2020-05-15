using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class MeasurementSerie : IMeasurementSerie
    {
        public IReadOnlyList<INumericMeasurementPoint> MeasuredPoints { get; }

        public Units Dimension { get; }

        public string Name { get; }

        public MeasurementSerie(string measuredquantityname, List<INumericMeasurementPoint> measData, Units dimension = Units.ADU)
        {
            Name = measuredquantityname;
            MeasuredPoints = measData;
            Dimension = dimension;
        }


        public INumericMeasurementPoint this[int i] => MeasuredPoints[i];

    }
}
