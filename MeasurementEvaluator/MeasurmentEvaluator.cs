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
    internal class MeasurmentEvaluator
    {

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;



        private static ILogger _logger;
        private static ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);

        private static string SpecificationFolder { get; set; }
        private static string ReferenceFolderPath { get; set; }
        private static string MeasurementDataFolder { get; set; }
        private static string ResultFolder { get; set; }
        private static string PluginsFolder { get; set; }
        private static string CurrentExeFolder { get; set; }


        static void Main(string[] args)
        {
            try
            {
                _logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception)
            {
                Console.WriteLine("Logger could not been created.");
                return;
            }

            try
            {
                CurrentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                SendToInfoLogAndConsole($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                SendToInfoLogAndConsole($"Application runtime folder: {CurrentExeFolder}");

                // read some data from App settings
                if (!ReadConfig())
                {
                    return;
                }

                // frame start:
                if (!Frame.PluginLoader.PluginLoader.SetPluginFolder(PluginsFolder))
                {
                    SendToErrrorLogAndConsole("Frame setup was not successful.");
                    return;
                }

                // Start UI:
                Thread appThread = new Thread(() =>
                {
                    Application application = new Application();

                    MainWindow mainWindow = new MainWindow() { Title = "Measurement Evaluator UI" };
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


        private static void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            // todo: null mainwindow??
            SendToInfoLogAndConsole("MainWindow closed.");

            Task.Run(() => _uiFinishedEvent.Set());
        }


        private static bool ReadConfig()
        {
            try
            {
                SpecificationFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["SpecificationFolder"], nameof(SpecificationFolder));
                if (SpecificationFolder == null)
                {
                    return false;
                }

                ReferenceFolderPath = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["ReferenceFolder"], nameof(ReferenceFolderPath));
                if (ReferenceFolderPath == null)
                {
                    return false;
                }

                MeasurementDataFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["MeasurementFolder"], nameof(MeasurementDataFolder));
                if (MeasurementDataFolder == null)
                {
                    return false;
                }

                ResultFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["ResultFolder"], nameof(ResultFolder));
                if (MeasurementDataFolder == null)
                {
                    return false;
                }

                PluginsFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["PluginsFolder"], nameof(PluginsFolder));
                if (PluginsFolder == null)
                {
                    return false;
                }

                if (_logger.IsTraceEnabled)
                {
                    SendToInfoLogAndConsole($"{nameof(SpecificationFolder)}: {SpecificationFolder}");
                    SendToInfoLogAndConsole($"{nameof(ReferenceFolderPath)}: {ReferenceFolderPath}");
                    SendToInfoLogAndConsole($"{nameof(MeasurementDataFolder)}: {MeasurementDataFolder}");
                    SendToInfoLogAndConsole($"{nameof(ResultFolder)}: {ResultFolder}");
                    SendToInfoLogAndConsole($"{nameof(PluginsFolder)}: {PluginsFolder}");
                }

                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                _logger.Error($"Problem during configuration folder loading from App settings: {ex.Message}");
                return false;
            }
        }

        private static string CreateFinalPath(string currentExeFolder, string specificationFolder, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(currentExeFolder) || string.IsNullOrEmpty(name))
                {
                    SendToErrrorLogAndConsole("Received exefolder-path OR path-name is null.");
                    return null;
                }

                if (string.IsNullOrEmpty(specificationFolder))
                {
                    SendToErrrorLogAndConsole($"{name} is null.");
                    return null;
                }

                if (Path.IsPathRooted(specificationFolder))
                {
                    if (!Directory.Exists(specificationFolder))
                    {
                        Directory.CreateDirectory(specificationFolder);
                        SendToInfoLogAndConsole($"{specificationFolder} created.");
                    }

                    SendToInfoLogAndConsole($"{name} ({specificationFolder}) wil be used.");
                    return specificationFolder;
                }

                string combinedPath = Path.Combine(currentExeFolder, specificationFolder);

                if (!Directory.Exists(combinedPath))
                {
                    SendToInfoLogAndConsole($"Combined {name} ({combinedPath}) created.");
                    Directory.CreateDirectory(combinedPath);
                }

                SendToInfoLogAndConsole($"Combined {name} ({combinedPath}) will be used.");
                return combinedPath;
            }
            catch (Exception ex)
            {
                SendToErrrorLogAndConsole($"Problem during {name} check: {ex}");
                return null;
            }
        }


        private static void SendToInfoLogAndConsole(string message)
        {
            _logger.Info(message);
            Console.WriteLine(message + Environment.NewLine);
        }

        private static void SendToErrrorLogAndConsole(string message)
        {
            _logger.Error(message);
            Console.WriteLine(message + Environment.NewLine);
        }

    }
}
