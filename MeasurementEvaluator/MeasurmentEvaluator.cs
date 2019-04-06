﻿using Frame.ConfigHandler;
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

        [Configuration("Name of the used main window", "MainWindow Name", true)]
        private string _mainWindowName = null;
        private IWindowUIWPF _window;


        [Configuration("true -> the application does not start, only some dummy objects will be (re)created.", "Create Dummy Objects", true)]
        private bool _createDummyObjects = false;

        public MeasurmentEvaluator()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }


        public void Run()
        {
            try
            {
                bool successfulLoading = PluginLoader.ConfigManager.Load(this, nameof(MeasurementEvaluator));
                if (!successfulLoading)
                {
                    PluginLoader.SendToErrrorLogAndConsole($"Loading of {nameof(MeasurementEvaluator)} was not successful in the {nameof(PluginLoader)}.");
                }

                if (_createDummyObjects)
                {
                    IDummyObjectCreator creator = PluginLoader.CreateInstance<IDummyObjectCreator>("DummyObjectCreator");

                    creator.Create(PluginLoader.SpecificationFolder, PluginLoader.ReferenceFolder, PluginLoader.MeasurementDataFolder);

                    _logger.Info("**********************************************************************************************************************************");

                    _logger.Info("DummyObject Creator started. Folders:");
                    _logger.Info($"Specification Folder: {PluginLoader.SpecificationFolder}");
                    _logger.Info($"Reference Folder: {PluginLoader.ReferenceFolder}");
                    _logger.Info($"Meaurement Folder: {PluginLoader.MeasurementDataFolder}");

                    _logger.Info("**********************************************************************************************************************************");

                    return;
                }

                // Start UI:
                Thread appThread = new Thread(() =>
                {
                    _window = PluginLoader.CreateInstance<IWindowUIWPF>(_mainWindowName);

                    Application application = new Application();

                    Window mainWindow = (Window)_window;
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

                PluginLoader.SendToInfoLogAndConsole($"Current application ({Assembly.GetExecutingAssembly().FullName}) stopped.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            // todo: null mainwindow??
            PluginLoader.SendToInfoLogAndConsole("MainWindow closed.");

            Task.Run(() => _uiFinishedEvent.Set());
        }

    }
}
