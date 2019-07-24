using Interfaces.BaseClasses;
using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class MeasurementSerie : IMeasurementSerie
    {
        public IReadOnlyList<IMeasurementPoint> MeasuredPoints { get; }

        public Units Dimension { get; }

        public string Name { get; }

        public MeasurementSerie(string measuredquantityname, List<IMeasurementPoint> measData, Units dimension = Units.ADU)
        {
            Name = measuredquantityname;
            MeasuredPoints = measData;
            Dimension = dimension;
        }


        public IMeasurementPoint this[int i] => MeasuredPoints[i];

    }
}
