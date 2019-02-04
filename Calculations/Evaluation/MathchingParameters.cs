using Interfaces.DataAcquisition;
using NLog;

namespace Calculations.Evaluation
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
