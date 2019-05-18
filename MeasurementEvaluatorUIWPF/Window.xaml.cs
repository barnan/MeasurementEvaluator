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

            //Style style = Application.Current.FindResource("CustomWindowStyle") as Style;
            //this.Style = style;
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
