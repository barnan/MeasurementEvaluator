using System.Diagnostics;
using System.Reflection;
using Frame.Configuration;
using Frame.PluginLoader;
using FrameInterfaces;
using System.Windows;
using Interfaces.MeasurementEvaluator.Evaluation;
using Interfaces.Misc;
using Interfaces.IUIWPF;

namespace MeasurementEvaluator
{
    public class MeasurementEvaluator : IRunnable
    {
        private Application _application;
        private readonly IMyLogger _logger;
        private readonly ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _uiCreatedEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _blCreatedEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _blInitCompletedEvent = new ManualResetEvent(false);

        #region configuration

        [Configuration("Name of the used main window", "MainWindow Name", true)]
        private readonly string _mainWindowName = string.Empty;

        [Configuration("Name of the evaluator", "Evaluator Name", true)]
        private readonly string _evaluatorName = string.Empty;
        private IEvaluation Evaluator { get; set; }

        [Configuration("true -> the application does not start, only some dummy objects will be (re)created.", "Create Dummy Objects", false)]
        private bool _createDummyObjects = false;

        #endregion

        #region ctor

        /// <summary>
        /// ctor for the activator
        /// </summary>
        public MeasurementEvaluator()
        {
            _logger = PluginLoader.GetLogger(nameof(MeasurementEvaluator));
        }

        #endregion

        public void Run()
        {
            try
            {
                bool successfulLoading = PluginLoader.ConfigManager.Load(this, nameof(MeasurementEvaluator));
                if (!successfulLoading)
                {
                    _logger.Error($"Loading of {nameof(MeasurementEvaluator)} was not successful in the {nameof(PluginLoader)}.");
                }

                if (_createDummyObjects)
                {
                    IDummyObjectCreator creator = PluginLoader.CreateInstance<IDummyObjectCreator>("DummyObjectCreator");

                    creator.Create(PluginLoader.SpecificationFolder, PluginLoader.ReferenceFolder, PluginLoader.MeasurementDataFolder);

                    _logger.Info("**********************************************************************************************************************************");

                    _logger.Info("DummyObject Creator started. Folders:");
                    _logger.Info($"Specification Folder: {PluginLoader.SpecificationFolder}");
                    _logger.Info($"Reference Folder: {PluginLoader.ReferenceFolder}");
                    _logger.Info($"Measurement Folder: {PluginLoader.MeasurementDataFolder}");

                    _logger.Info("**********************************************************************************************************************************");

                    return;
                }

                // Start UI:
                Thread appThread = new Thread(() =>
                {
                    _blCreatedEvent.WaitOne();

                    IMyWindowUIWPF window = PluginLoader.CreateInstance<IMyWindowUIWPF>(_mainWindowName);

                    Window mainWindow = (Window)window;
                    mainWindow.Closed += MainWindow_OnClosed;

                    _application = new Application { MainWindow = mainWindow };
                    mainWindow.Show();

                    _uiCreatedEvent.Set();

                    _blInitCompletedEvent.WaitOne();

                    if (!window.IsInitializationCompleted)
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
                //Evaluator = PluginLoader.CreateInstance<IEvaluation>(_evaluatorName);

                _blCreatedEvent.Set();
                _uiCreatedEvent.WaitOne();

                if (!Evaluator.Initiailze())
                {
                    _logger.Error($"{nameof(Evaluator)} could not been initialized.");
                }

                _blInitCompletedEvent.Set();
                _uiFinishedEvent.WaitOne();

                _logger.Info($"{Assembly.GetExecutingAssembly().GetName().Name} was shut down.");

                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occurred: {ex}");
            }
        }

        private void _application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Error($"Dispatcher unhandled exception occurred: {sender} - {e.Exception.Message}");
        }

        private void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            _logger.Info("MainWindow closed.");

            Evaluator.Close();

            _application.Shutdown();

            _uiFinishedEvent.Set();
        }
    }
}