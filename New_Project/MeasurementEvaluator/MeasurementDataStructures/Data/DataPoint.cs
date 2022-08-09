using Interfaces.MeasurementEvaluator.MeasuredData;

namespace MeasurementDataStructures.Data
{
    public class DataPoint<T> : IDataPoint<T>
    {
        private bool _isValid;
        private T _value;
        private DateTime _creationTime;

        public bool IsValid => _isValid;

        public DateTime CreationTime => _creationTime;

        public T Value => _value;
    }

}
