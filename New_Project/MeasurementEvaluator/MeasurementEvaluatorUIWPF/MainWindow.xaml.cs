using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Interfaces.IUIWPF;

namespace MeasurementEvaluatorUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMyWindowUIWPF
    {
        public bool IsInitializationCompleted => InitializationCompleted();


        private MainWindowParameters Parameter { get; }

        internal MainWindow(MainWindowParameters param)
        {
            Parameter = param;
            Name = param.Name;

            InitializeComponent();

            DataContext = new MainWindowViewModel(param);
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

        private void Window_OnClosed(object sender, EventArgs e)
        {
            try
            {
                Parameter.OnClosed();
            }
            catch (Exception)
            {
                // what should be done here?
            }
        }
    }
}
