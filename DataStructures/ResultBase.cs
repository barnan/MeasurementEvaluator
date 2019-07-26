using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace DataStructures
{
    public abstract class ResultBase : IResult, IFormattable
    {
        protected ResultBase(DateTime creationTime, bool successful)
        {
            CreationTime = creationTime;
            Successful = successful;
        }

        public virtual DateTime CreationTime { get; }
        public virtual bool Successful { get; }

        public abstract XElement SaveToXml(XElement input);

        public abstract bool LoadFromXml(XElement input);

        public abstract string ToString(string format, IFormatProvider formatProvider);

        public override string ToString()
        {
            return $"CreationTime:{CreationTime}Successful{Environment.NewLine}{Successful}{Environment.NewLine}";
        }
    }
}
