using Interfaces.DataAcquisition;

namespace MeasurementEvaluatorUI.DataCollectorUIWpf
{

    internal class DataCollectorUIWPFParameters
    {
        internal IDataCollector DataCollector { get; }


        public DataCollectorUIWPFParameters(IDataCollector dataCollector)
        {
            DataCollector = dataCollector;
        }

    }
}
