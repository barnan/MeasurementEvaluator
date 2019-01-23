using Interfaces;
using Interfaces.MeasuredData;
using System.Collections.Generic;

namespace DataStructures.MeasuredData
{
    public class MeasurementSerie : IMeasurementSerie
    {
        public string MeasuredQuantityName { get; }

        public IReadOnlyList<IUniqueMeasurementResult> MeasData { get; }

        public Units Dimension { get; }


        public MeasurementSerie(string measuredquantityname, List<IUniqueMeasurementResult> measData, Units dimension = Units.ADU)
        {
            MeasuredQuantityName = measuredquantityname;
            MeasData = measData;
            Dimension = dimension;
        }


        public IUniqueMeasurementResult this[int i] => MeasData[i];
    }
}
