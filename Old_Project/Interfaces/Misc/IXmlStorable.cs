using System.Xml.Linq;

namespace Interfaces.Misc
{
    public interface IXmlStorable
    {
        /// <summary>
        /// Save the result content into an XElement
        /// </summary>
        /// <param name="inputElement">XElement to save the data into it</param>
        /// <returns>return the same XElement but filled with the content of the current object</returns>
        XElement SaveToXml(XElement inputElement);


        /// <summary>
        /// Load the necessry data from an XElement
        /// </summary>
        /// <param name="inputElement">XElement to load the data from it</param>
        /// <returns></returns>
        bool LoadFromXml(XElement inputElement);

    }
}
