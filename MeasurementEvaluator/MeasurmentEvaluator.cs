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

        [Configuration("Name of the used main window", "MainWindow Name", true)]
        private string _mainWindowName = null;

        [Configuration("Name of the evaluator", "Evaluator Name", true)]
        private string _dataEvaluatorName = null;
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

                Evaluator = PluginLoader.CreateInstance<IEvaluation>(_dataEvaluatorName);
                if (!Evaluator.Initiailze())
                {
                    PluginLoader.SendToErrorLogAndConsole($"{nameof(Evaluator)} could not been initialized.");
                }

                // Start UI:
                Thread appThread = new Thread(() =>
                {
                    _application = new Application();
                    //var myResourceDictionary = new ResourceDictionary { Source = new Uri("/MeasurementEvaluatorUIWPF;component/Themes/Styles.xaml", UriKind.RelativeOrAbsolute) };
                    //application.Resources.MergedDictionaries.Add(myResourceDictionary);

                    _window = PluginLoader.CreateInstance<IWindowUIWPF>(_mainWindowName);

                    Window mainWindow = (Window)_window;
                    mainWindow.Closed += MainWindow_OnClosed;

                    //System.Windows.Application.Current.MainWindow = mainWindow;
                    _application.MainWindow = mainWindow;
                    mainWindow.Show();

                    if (!_window.InitializationCompleted())
                    {
                        _logger.Error("InitializationCompleted failed.");
                        return;
                    }

                    _application.Run(mainWindow);

                });
                Debug.Assert(appThread != null);
                appThread.Name = "WpfThread";
                appThread.SetApartmentState(ApartmentState.STA);
                appThread.IsBackground = true;
                appThread.Start();


                _uiFinishedEvent.WaitOne();
                PluginLoader.SendToInfoLogAndConsole($"Current application ({Assembly.GetExecutingAssembly().FullName}) stopped.");

            }
            catch (Exception ex)
            {
                PluginLoader.SendToErrorLogAndConsole($"Exception occured: {ex}");
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
