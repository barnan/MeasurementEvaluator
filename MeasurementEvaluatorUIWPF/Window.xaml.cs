using Interfaces.Misc;

namespace MeasurementEvaluatorUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window : System.Windows.Window, IWindowUIWPF
    {
        private MainWindowParameters _param;

        internal Window(MainWindowParameters param)
        {
            _param = param;
            Name = param.ID;

            InitializeComponent();

            this.DataContext = new MainWindowViewModel(param);
        }
    }
}
