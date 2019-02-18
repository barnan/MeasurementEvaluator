using MeasurementEvaluatorUI.Base;

namespace MeasurementEvaluatorUI.SampleOrientationUIWPF
{
    /// <summary>
    /// Interaction logic for SampleOrientationUIWPF.xaml
    /// </summary>
    public partial class SampleOrientationUIWPF : UserControlBase
    {
        public SampleOrientationUIWPF()
        {
            InitializeComponent();
            var viewModel = new SampleOrientationViewModel();
            DataContext = viewModel;
        }




    }
}
