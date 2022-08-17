using System.Xml.Linq;
using Interfaces.MeasurementEvaluator.Result;

namespace MeasurementDataStructures
{
    public abstract class ResultBase : IResult
    {
        protected ResultBase(DateTime creationTime, bool successful)
        {
            CreationTime = creationTime;
            IsSuccessful = successful;
        }

        public virtual DateTime CreationTime { get; }

        public virtual bool IsSuccessful { get; }

        public abstract XElement SaveToXml(XElement input);

        public abstract bool LoadFromXml(XElement input);

        public override string ToString()
        {
            return $"CreationTime:{CreationTime}Successful{Environment.NewLine}{IsSuccessful}{Environment.NewLine}";
        }
    }
}
