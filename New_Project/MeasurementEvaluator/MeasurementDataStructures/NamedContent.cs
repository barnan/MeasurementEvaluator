using Interfaces.Misc;

namespace MeasurementDataStructures
{
    public class NamedContent<T> : INamedContent<T>
    {
        public string Name { get; set; }

        public T Content { get; set; }
    }
}
