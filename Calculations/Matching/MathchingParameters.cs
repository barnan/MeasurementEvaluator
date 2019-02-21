using Interfaces.DataAcquisition;
using NLog;

namespace Calculations.Matching
{
    internal class MathchingParameters
    {
        internal ILogger Logger { get; }

        internal IHDDXmlSerializator XmlSerializator { get; }

        internal string NameBindingFilePath { get; }


        public MathchingParameters(IHDDXmlSerializator xmlSerializator, string nameBindingFilePath)
        {
            Logger = LogManager.GetCurrentClassLogger();
            XmlSerializator = xmlSerializator;
            NameBindingFilePath = nameBindingFilePath;
        }

    }

}
