using BaseClasses.MeasurementEvaluator;
using Interfaces.MeasurementEvaluator.MeasuredData;

namespace MeasurementDataStructures.Data
{
    public class MeasurementSerie : IMeasurementSerie
    {

        public MeasurementSerie(Units dimension, IReadOnlyList<IDataPoint<double>> dataPoints, string name)
        {
            Dimension = dimension;
            DataPoints = dataPoints;
            Name = name;
        }

        public string Name { get; } 

        IReadOnlyList<IDataPoint> IDataSerie.DataPoints => DataPoints;

        public IDataPoint<double> this[int i] => DataPoints[i];
        public IReadOnlyList<IDataPoint<double>> DataPoints { get; }
        IDataPoint IDataSerie.this[int i] => DataPoints.ToArray()[i];
        public Units Dimension { get; }
    }
}
