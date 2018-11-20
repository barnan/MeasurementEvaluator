using System.Xml;

namespace Interfaces.DataAcquisition
{
    public interface IXmlReader
    {
        XmlDocument ReadFile(string fileFullPath);

    }
}
