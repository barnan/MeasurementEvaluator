using System.Diagnostics;
using System.Reflection;
using Frame.PluginLoader;
using FrameInterfaces;

namespace Starter
{
    internal class Program
    {
        private static string _currentExeFolder;
        private static IMyLogger _logger;

        static void Main(string[] args)
        {
            try
            {
                _logger = PluginLoader.GetLogger("Starter");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logger could not been created: {ex.Message}");
                return;
            }

            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                _logger.Info($"Process {currentProcess} started");

                _currentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                _logger.Info($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                _logger.Info($"Application runtime folder: {_currentExeFolder}");

                // frame start:
                PluginLoader pluginLoader = new PluginLoader();
                if (!pluginLoader.Inititialize(_currentExeFolder))
                {
                    _logger.Error("Frame initialization was NOT successful");
                    return;
                }
                pluginLoader.Start();

                _logger.Info($"Process {currentProcess} ended");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occurred: {ex}");
            }
        }

    }
}