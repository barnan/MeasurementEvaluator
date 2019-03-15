﻿using Frame.PluginLoader.Interfaces;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeasurementEvaluator
{
    internal class MeasurmentEvaluator : IRunable
    {

        //[DllImport("kernel32.dll")]
        //internal static extern IntPtr GetConsoleWindow();


        //[DllImport("user32.dll")]
        //internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        //const int SW_HIDE = 0;
        //const int SW_SHOW = 5;


        private ILogger _logger;
        private ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);


        MainWindow


        public MeasurmentEvaluator()
        {
            _logger = LogManager.GetCurrentClassLogger();

        }



        public bool Run()
        {


            return true;
        }



        //        static void Main(string[] args)
        //        {
        //            try
        //            {
        //                _logger = LogManager.GetCurrentClassLogger();
        //            }
        //            catch (Exception)
        //            {
        //                Console.WriteLine("Logger could not been created.");
        //                return;
        //            }

        //            try
        //            {
        //                CurrentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //                SendToInfoLogAndConsole($"Application started: {Assembly.GetExecutingAssembly().FullName}");
        //                SendToInfoLogAndConsole($"Application runtime folder: {CurrentExeFolder}");

        //                // read some data from App settings
        //                if (!ReadConfig())
        //                {
        //                    return;
        //                }

        //                // frame start:
        //                //if (!Frame.PluginLoader.PluginLoader)
        //                //{
        //                //    SendToErrrorLogAndConsole("Frame setup was not successful.");
        //                //    return;
        //                //}

        //                // Start UI:
        //                Thread appThread = new Thread(() =>
        //                {
        //                    Application application = new Application();

        //                    //MainWindow mainWindow = new MainWindow() { Title = "Measurement Evaluator UI" };
        //                    //mainWindow.Closed += MainWindow_OnClosed;

        //                    //System.Windows.Application.Current.MainWindow = mainWindow;
        //                    //mainWindow.Show();
        //                    //application.Run(mainWindow);

        //                });
        //                Debug.Assert(appThread != null);
        //                appThread.Name = "WpfThread";
        //                appThread.SetApartmentState(ApartmentState.STA);
        //                appThread.IsBackground = true;
        //                appThread.Start();

        //#if RELEASE
        //                ShowWindow(GetConsoleWindow(), SW_HIDE);
        //#endif               

        //                _uiFinishedEvent.WaitOne();

        //                SendToInfoLogAndConsole($"Current application ({Assembly.GetExecutingAssembly().FullName}) stopped.");
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.Error($"Exception occured: {ex}");
        //            }
        //        }


        private void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            // todo: null mainwindow??
            SendToInfoLogAndConsole("MainWindow closed.");

            Task.Run(() => _uiFinishedEvent.Set());
        }


        //private static bool ReadConfig()
        //{
        //    try
        //    {
        //        SpecificationFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["SpecificationFolder"], nameof(SpecificationFolder));
        //        if (SpecificationFolder == null)
        //        {
        //            return false;
        //        }

        //        ReferenceFolderPath = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["ReferenceFolder"], nameof(ReferenceFolderPath));
        //        if (ReferenceFolderPath == null)
        //        {
        //            return false;
        //        }

        //        MeasurementDataFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["MeasurementFolder"], nameof(MeasurementDataFolder));
        //        if (MeasurementDataFolder == null)
        //        {
        //            return false;
        //        }

        //        ResultFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["ResultFolder"], nameof(ResultFolder));
        //        if (MeasurementDataFolder == null)
        //        {
        //            return false;
        //        }

        //        PluginsFolder = CreateFinalPath(CurrentExeFolder, System.Configuration.ConfigurationManager.AppSettings["PluginsFolder"], nameof(PluginsFolder));
        //        if (PluginsFolder == null)
        //        {
        //            return false;
        //        }

        //        if (_logger.IsTraceEnabled)
        //        {
        //            SendToInfoLogAndConsole($"{nameof(SpecificationFolder)}: {SpecificationFolder}");
        //            SendToInfoLogAndConsole($"{nameof(ReferenceFolderPath)}: {ReferenceFolderPath}");
        //            SendToInfoLogAndConsole($"{nameof(MeasurementDataFolder)}: {MeasurementDataFolder}");
        //            SendToInfoLogAndConsole($"{nameof(ResultFolder)}: {ResultFolder}");
        //            SendToInfoLogAndConsole($"{nameof(PluginsFolder)}: {PluginsFolder}");
        //        }

        //        return true;
        //    }
        //    catch (ConfigurationErrorsException ex)
        //    {
        //        _logger.Error($"Problem during configuration folder loading from App settings: {ex.Message}");
        //        return false;
        //    }
        //}



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