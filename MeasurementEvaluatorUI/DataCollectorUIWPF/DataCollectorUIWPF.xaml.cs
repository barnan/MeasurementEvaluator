using MeasurementEvaluatorUI.DataCollectorUIWPF;
using System.Windows.Controls;

namespace MeasurementEvaluatorUI.DataCollectorUIWpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            var viewModel = new DataCollectorViewModel();
            DataContext = viewModel;

        }
    }
}
