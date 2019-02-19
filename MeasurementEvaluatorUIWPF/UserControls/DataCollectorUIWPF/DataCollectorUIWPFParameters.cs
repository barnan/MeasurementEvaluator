using Interfaces.DataAcquisition;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{

    public class DataCollectorUIWPFParameters
    {
        internal IDataCollector DataCollector { get; }


        public DataCollectorUIWPFParameters(IDataCollector dataCollector)
        {
            DataCollector = dataCollector;
        }

    }
}
