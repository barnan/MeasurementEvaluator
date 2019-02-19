using Interfaces.DataAcquisition;

namespace MeasurementEvaluatorUI.DataCollectorUIWPF
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
