using Interfaces.Misc;
using MahApps.Metro.Controls;
using System;

namespace MeasurementEvaluatorUIWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window : MetroWindow, IWindowUIWPF
    {
        private MainWindowParameters Parameter { get; }

        internal Window(MainWindowParameters param)
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
            }
        }
    }
}
