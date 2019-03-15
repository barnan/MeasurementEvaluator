using Interfaces.Misc;
using System.Windows;

namespace MeasurementEvaluatorUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowUIWPF
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(new MainWindowParameters());
        }
    }
}
