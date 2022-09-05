using Interfaces.IUIWPF;
using MeasurementEvaluatorUIWPF.Base;

namespace MeasurementEvaluatorUIWPF.UserControls.DataCollectorUIWPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DataCollectorUIWPF : UserControlBase, IUserControlUIWPF
    {
        public DataCollectorUIWPF(DataCollectorUIWPFParameters param)
        {
            InitializeComponent();

            var viewModel = new DataCollectorViewModel(param);
            DataContext = viewModel;
            Name = "Collect Data";

        }
    }


}
