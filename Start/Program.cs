using Frame.PluginLoader;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Start
{
    internal class Program
    {
        private static ILogger _logger;

        private static string _configurationFolder;
        private static string _specificationFolder;
        private static string _referenceFolderPath;
        private static string _measurementDataFolder;
        private static string _resultFolder;
        private static string _pluginsFolder;
        private static string _currentExeFolder;


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
                Process currentprocess = Process.GetCurrentProcess();

                _currentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (string.IsNullOrEmpty(_currentExeFolder))
                {
                    PluginLoader.SendToErrorLogAndConsole("Received exefolder-path OR path-name is null.");
                    return;
                }

                PluginLoader.SendToInfoLogAndConsole($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                PluginLoader.SendToInfoLogAndConsole($"Application runtime folder: {_currentExeFolder}");

                // frame start:
                PluginLoader pluginLoader = new PluginLoader();
                if (!pluginLoader.Inititialize(_currentExeFolder))
                {
                    PluginLoader.SendToErrorLogAndConsole("Frame initialization was NOT successful.");
                    return;
                }
                pluginLoader.Start();

                PluginLoader.SendToInfoLogAndConsole($"Process {currentprocess} ended.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }
        }

    }
}
