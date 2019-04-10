using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace DataStructures
{
    public abstract class ResultBase : IResult
    {
        protected ResultBase(DateTime startTime, DateTime endTime, bool successful)
        {
            StartTime = startTime;
            EndTime = endTime;
            Successful = successful;
        }

        public virtual DateTime StartTime { get; }
        public virtual DateTime EndTime { get; }
        public virtual bool Successful { get; }

        public XElement SaveToXml(XElement input)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromXml(XElement input)
        {
            throw new NotImplementedException();
        }
    }
}
