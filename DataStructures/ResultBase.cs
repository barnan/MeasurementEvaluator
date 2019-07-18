using Interfaces.Result;
using System;
using System.Xml.Linq;

namespace DataStructures
{
    public abstract class ResultBase : IResult
    {
        protected ResultBase(DateTime creationTime, bool successful)
        {
            CreationTime = creationTime;
            Successful = successful;
        }

        public virtual DateTime CreationTime { get; }
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
