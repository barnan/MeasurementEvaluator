using MeasurementEvaluatorUIWPF.Base;

namespace MeasurementEvaluatorUI.UserControls.SampleOrientationUIWPF
{
    public class SampleOrientationParameters : ParameterBase
    {
        public SampleOrientationParameters(string sectionName)
        {
        }

        internal bool Load(string sectionName)
        {
            Name = sectionName;

            return true;
        }

    }
}
