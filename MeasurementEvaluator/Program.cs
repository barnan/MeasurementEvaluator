using MeasurementEvaluatorUIWPF;
using NLog;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MeasurementEvaluator
{



    public class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;



        private static ILogger _logger;
        private static ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);

        public static string SpecificationFolderPath { get; private set; }
        public static string ReferenceFolderPath { get; private set; }
        public static string MeasurementDataFolderPath { get; private set; }
        public static string ResultFolderPath { get; private set; }
        public static string CurrentExeFolder { get; private set; }



        static void Main(string[] args)
        {
            try
            {
                _logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                return;
            }

            try
            {
                CurrentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                _logger.Info($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                _logger.Info($"Application runtime folder: {CurrentExeFolder}");

                if (!ReadConfig())
                {
                    return;
                }

                Thread appThread = new Thread(() =>
                {
                    Application application = new Application();

                    MainWindow mainWindow = new MainWindow() { Title = "Measurement Evaluator" };
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

            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }
        }


        private static bool ReadConfig()
        {
            try
            {
                SpecificationFolderPath = System.Configuration.ConfigurationManager.AppSettings["SpecificationFolder"];
                ReferenceFolderPath = System.Configuration.ConfigurationManager.AppSettings["ReferenceFolder"];
                MeasurementDataFolderPath = System.Configuration.ConfigurationManager.AppSettings["MeasurementFolder"];
                ResultFolderPath = System.Configuration.ConfigurationManager.AppSettings["ResultFolder"];

                if (!CheckFolder(SpecificationFolderPath, nameof(SpecificationFolderPath)))
                {
                    return false;
                }
                if (!CheckFolder(ReferenceFolderPath, nameof(ReferenceFolderPath)))
                {
                    return false;
                }
                if (!CheckFolder(MeasurementDataFolderPath, nameof(MeasurementDataFolderPath)))
                {
                    return false;
                }
                if (!CheckFolder(ResultFolderPath, nameof(ResultFolderPath)))
                {
                    return false;
                }

                if (_logger.IsTraceEnabled)
                {
                    _logger.Info($"{nameof(SpecificationFolderPath)}: {SpecificationFolderPath}");
                    _logger.Info($"{nameof(ReferenceFolderPath)}: {ReferenceFolderPath}");
                    _logger.Info($"{nameof(MeasurementDataFolderPath)}: {MeasurementDataFolderPath}");
                    _logger.Info($"{nameof(ResultFolderPath)}: {ResultFolderPath}");
                }

                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                _logger.Error($"Problem during configuration loadin: {ex.Message}");
                return false;
            }
        }


        private static bool CheckFolder(string content, string name)
        {
            if (string.IsNullOrEmpty(content))
            {
                _logger.Error($"{name} is null.");
                return false;
            }
            return true;
        }


        private static void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            Task.Run(() => _uiFinishedEvent.Set());
        }
    }
}
