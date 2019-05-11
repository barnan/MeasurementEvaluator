using Interfaces.Misc;
using System;

namespace MeasurementEvaluatorUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window : System.Windows.Window, IWindowUIWPF
    {
        private MainWindowParameters Parameter { get; }

        internal Window(MainWindowParameters param)
        {
            Parameter = param;
            Name = param.Name;

            InitializeComponent();

            this.DataContext = new MainWindowViewModel(param);
        }

        public bool InitializationCompleted()
        {
            try
            {
                Parameter.OnInitializationCompleted();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
