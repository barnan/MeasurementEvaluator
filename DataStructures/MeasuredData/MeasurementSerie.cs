using Interfaces;
using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class MeasurementSerie : IMeasurementSerie
    {
        public string Name { get; }

        public List<IUniqueMeasurementResult> MeasData { get; }

        public Units Dimension { get; }


        public MeasurementSerie(string name, List<IUniqueMeasurementResult> measData, Units dimension = Units.ADU)
        {
            Name = name;
            MeasData = measData;
            Dimension = dimension;
        }


        public IUniqueMeasurementResult this[int i]
        {
            get { return MeasData[i]; }
        }

    }
}
