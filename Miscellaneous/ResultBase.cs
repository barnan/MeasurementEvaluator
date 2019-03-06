using System;
using System.Xml;

namespace Miscellaneous
{
    public abstract class ResultBase
    {
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
