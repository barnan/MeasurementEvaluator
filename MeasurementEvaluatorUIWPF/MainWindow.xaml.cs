using Interfaces.Misc;
using System.Windows;

namespace MeasurementEvaluatorUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowUIWPF
    {
        private MainWindowParameters _param;

        internal MainWindow(MainWindowParameters param)
        {
            _param = param;

            InitializeComponent();

            this.DataContext = new MainWindowViewModel(new MainWindowParameters());
        }
    }
}
