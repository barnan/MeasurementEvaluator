using Frame.ConfigHandler;
using Frame.PluginLoader;
using Frame.PluginLoader.Interfaces;
using Interfaces.Misc;
using NLog;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MeasurementEvaluator
{
    internal class MeasurmentEvaluator : IRunable
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        private ILogger _logger;
        private ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);

        [Configuration("Title of the used main window", "MainWindow", true)]
        private string _mainWindowName = null;

        private IMainWindowUIWPF _mainWindow;

        public MeasurmentEvaluator()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }


        public void Run()
        {
            try
            {
                bool successfulLoading = PluginLoader.ConfigManager.Load(this, nameof(MeasurementEvaluator));
                _mainWindow = PluginLoader.CreateInstance<IMainWindowUIWPF>(typeof(IMainWindowUIWPF), _mainWindowName);

                // Start UI:
                Thread appThread = new Thread(() =>
                {
                    Application application = new Application();

                    Window mainWindow = (Window)_mainWindow;
                    mainWindow.Closed += MainWindow_OnClosed;

                    System.Windows.Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                    application.Run(mainWindow);

                });
                Debug.Assert(appThread != null);
                appThread.Name = "WpfThread";
                appThread.SetApartmentState(ApartmentState.STA);
                appThread.IsBackground = true;
                appThread.Start();

#if RELEASE
                            ShowWindow(GetConsoleWindow(), SW_HIDE);
#endif

                _uiFinishedEvent.WaitOne();

                SendToInfoLogAndConsole($"Current application ({Assembly.GetExecutingAssembly().FullName}) stopped.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            // todo: null mainwindow??
            SendToInfoLogAndConsole("MainWindow closed.");

            Task.Run(() => _uiFinishedEvent.Set());
        }


        private void SendToInfoLogAndConsole(string message)
        {
            _logger.Info(message);
            Console.WriteLine(message + Environment.NewLine);
        }

        private void SendToErrrorLogAndConsole(string message)
        {
            _logger.Error(message);
            Console.WriteLine(message + Environment.NewLine);
        }


    }
}
