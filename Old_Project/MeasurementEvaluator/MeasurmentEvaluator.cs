using Frame.ConfigHandler;
using Frame.PluginLoader;
using Frame.PluginLoader.Interfaces;
using Interfaces.Evaluation;
using Interfaces.Misc;
using NLog;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace MeasurementEvaluator
{
    internal class MeasurmentEvaluator : IRunable
    {
        private Application _application;
        private readonly ILogger _logger;
        private readonly ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _uiCreatedEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _blCreatedEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _blInitCompletedEvent = new ManualResetEvent(false);


        #region configuration

        [Configuration("Name of the used main window", "MainWindow Name", true)]
        private readonly string _mainWindowName = null;

        [Configuration("Name of the evaluator", "Evaluator Name", true)]
        private readonly string _evaluatorName = null;
        private IEvaluation Evaluator { get; set; }

        [Configuration("true -> the application does not start, only some dummy objects will be (re)created.", "Create Dummy Objects", true)]
        private bool _createDummyObjects = false;

        #endregion


        #region ctor

        /// <summary>
        /// ctor for the activator
        /// </summary>
        public MeasurmentEvaluator()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        #endregion

        #region IRunable

        public void Run()
        {
            try
            {
                bool successfulLoading = PluginLoader.ConfigManager.Load(this, nameof(MeasurementEvaluator));
                if (!successfulLoading)
                {
                    SendToErrorLogAndConsole($"Loading of {nameof(MeasurementEvaluator)} was not successful in the {nameof(PluginLoader)}.");
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
                    _blCreatedEvent.WaitOne();

                    IWindowUIWPF window = PluginLoader.CreateInstance<IWindowUIWPF>(_mainWindowName);

                    Window mainWindow = (Window)window;
                    mainWindow.Closed += MainWindow_OnClosed;

                    _application = new Application { MainWindow = mainWindow };
                    mainWindow.Show();

                    _uiCreatedEvent.Set();

                    _blInitCompletedEvent.WaitOne();

                    if (!window.InitializationCompleted())
                    {
                        _logger.Error("InitializationCompleted event failed.");
                        return;
                    }

                    _application.DispatcherUnhandledException += _application_DispatcherUnhandledException;
                    _application.Run(mainWindow);

                });
                Debug.Assert(appThread != null);
                appThread.Name = "WPF_Thread";
                appThread.SetApartmentState(ApartmentState.STA);
                appThread.IsBackground = true;
                appThread.Start();

                // create evaluator
                Evaluator = PluginLoader.CreateInstance<IEvaluation>(_evaluatorName);

                _blCreatedEvent.Set();
                _uiCreatedEvent.WaitOne();

                if (!Evaluator.Initiailze())
                {
                    SendToErrorLogAndConsole($"{nameof(Evaluator)} could not been initialized.");
                }

                _blInitCompletedEvent.Set();
                _uiFinishedEvent.WaitOne();

                SendToInfoLogAndConsole($"{Assembly.GetExecutingAssembly().GetName().Name} was shut down.");

                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                SendToErrorLogAndConsole($"Exception occured: {ex}");
            }
        }

        #endregion

        #region private

        private void _application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            PluginLoader.ErrorLog($"Exception occured: {sender} - {e.Exception.Message}");
        }

        private void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            PluginLoader.InfoLog("MainWindow closed.");

            Evaluator.Close();

            _application.Shutdown();

            _uiFinishedEvent.Set();
        }


        private void SendToErrorLogAndConsole(string message)
        {
            PluginLoader.InfoLog(message);
        }

        private void SendToInfoLogAndConsole(string message)
        {
            PluginLoader.InfoLog(message);
        }


        #endregion

    }
}
