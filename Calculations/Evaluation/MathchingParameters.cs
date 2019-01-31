using Interfaces.DataAcquisition;
using NLog;

namespace Calculations.Evaluation
{
    internal class MathchingParameters
    {
        internal ILogger Logger { get; }

        internal IHDDXmlReader XmlReader { get; }

        internal string NameBindingFilePath { get; }



        public MathchingParameters(IHDDXmlReader xmlReader, string nameBindingFilePath)
        {
            Logger = LogManager.GetCurrentClassLogger();
            XmlReader = xmlReader;
            NameBindingFilePath = nameBindingFilePath;
        }

    }

}
