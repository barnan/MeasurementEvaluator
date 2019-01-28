using Interfaces.DataAcquisition;
using NLog;

namespace Calculations.Evaluation
{
    internal class MathchingParameters
    {
        internal ILogger Logger { get; }

        internal IHDDXmlReader XmlReader { get; }

        internal string MeasurementDataSpecificationFilePath { get; }

        internal string MeasurementDataReferenceFilePath { get; }



        public MathchingParameters(IHDDXmlReader xmlReader, string measurementDataSpecificationFilePath, string measurementDataReferenceFilePath)
        {
            Logger = LogManager.GetCurrentClassLogger();
            XmlReader = xmlReader;
            MeasurementDataSpecificationFilePath = measurementDataSpecificationFilePath;
            MeasurementDataReferenceFilePath = measurementDataReferenceFilePath;
        }

    }

}
