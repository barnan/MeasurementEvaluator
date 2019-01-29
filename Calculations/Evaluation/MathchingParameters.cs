using Interfaces.DataAcquisition;
using NLog;

namespace Calculations.Evaluation
{
    internal class MathchingParameters
    {
        internal ILogger Logger { get; }

        internal IHDDXmlReader XmlReader { get; }

        internal string MeasurementDataSpecificationReferenceFilePath { get; }



        public MathchingParameters(IHDDXmlReader xmlReader, string measurementDataSpecificationReferenceFilePath)
        {
            Logger = LogManager.GetCurrentClassLogger();
            XmlReader = xmlReader;
            MeasurementDataSpecificationReferenceFilePath = measurementDataSpecificationReferenceFilePath;
        }

    }

}
