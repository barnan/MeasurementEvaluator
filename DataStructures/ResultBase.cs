using Interfaces.Result;
using System;
using System.Xml;

namespace DataStructures
{
    abstract class ResultBase : IResult
    {
        public abstract DateTime StartTime { get; }
        public abstract DateTime EndTime { get; }
        public abstract bool Successful { get; }

        public XmlElement Save(XmlElement input)
        {
            throw new NotImplementedException();
        }

        public bool Load(XmlElement input)
        {
            throw new NotImplementedException();
        }
    }
}
