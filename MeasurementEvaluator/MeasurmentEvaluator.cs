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
using System.Threading.Tasks;
using System.Windows;

namespace MeasurementEvaluator
{
    internal class MeasurmentEvaluator : IRunable
    {
        private Application _application;
        private readonly ILogger _logger;
        private readonly ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);

        private readonly ManualResetEvent _windowCreatedEvent = new ManualResetEvent(false);

        private readonly ManualResetEvent _initCompletedEvent = new ManualResetEvent(false);

        [Configuration("Name of the used main window", "MainWindow Name", true)]
        private readonly string _mainWindowName = null;

        [Configuration("Name of the evaluator", "Evaluator Name", true)]
        private readonly string _dataEvaluatorName = null;
        private IEvaluation Evaluator { get; set; }


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
                    PluginLoader.SendToErrorLogAndConsole($"Loading of {nameof(MeasurementEvaluator)} was not successful in the {nameof(PluginLoader)}.");
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
                    _application = new Application();

                    _window = PluginLoader.CreateInstance<IWindowUIWPF>(_mainWindowName);

                    _windowCreatedEvent.Set();

                    Window mainWindow = (Window)_window;
                    mainWindow.Closed += MainWindow_OnClosed;

                    _application.MainWindow = mainWindow;
                    mainWindow.Show();

                    _initCompletedEvent.WaitOne(9);

                    if (!_window.InitializationCompleted())
                    {
                        _logger.Error("InitializationCompleted event failed.");
                        return;
                    }

                    _application.Run(mainWindow);

                });
                Debug.Assert(appThread != null);
                appThread.Name = "ApplicationThread";
                appThread.SetApartmentState(ApartmentState.STA);
                appThread.IsBackground = true;
                appThread.Start();

                _windowCreatedEvent.WaitOne();  // to have instantiated message controller UI

                // create evaluator
                Evaluator = PluginLoader.CreateInstance<IEvaluation>(_dataEvaluatorName);
                if (!Evaluator.Initiailze())
                {
                    PluginLoader.SendToErrorLogAndConsole($"{nameof(Evaluator)} could not been initialized.");
                }

                _initCompletedEvent.Set();

                _uiFinishedEvent.WaitOne();

                PluginLoader.SendToInfoLogAndConsole($"{Assembly.GetExecutingAssembly().GetName().Name} was shut down.");

                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                PluginLoader.SendToErrorLogAndConsole($"Exception occured: {ex}");
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs eventArgs)
        {
            PluginLoader.SendToInfoLogAndConsole("MainWindow closed.");

            Evaluator.Close();

            _application.Shutdown();

            Task.Run(() => _uiFinishedEvent.Set());
        }

    }
}
