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
        private static string _currentExeFolder;


        static void Main(string[] args)
        {
            try
            {
                _logger = LogManager.GetCurrentClassLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logger could not been created: {ex.Message}");
                return;
            }

            try
            {
                Process currentprocess = Process.GetCurrentProcess();
                InfoLog($"Process {currentprocess} started.");

                _currentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                InfoLog($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                InfoLog($"Application runtime folder: {_currentExeFolder}");

                // frame start:
                PluginLoader pluginLoader = new PluginLoader();
                if (!pluginLoader.Inititialize(_currentExeFolder))
                {
                    ErrorLog("Frame initialization was NOT successful.");
                    return;
                }
                pluginLoader.Start();

                InfoLog($"Process {currentprocess} ended.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }
        }



        private static void InfoLog(string message)
        {
            PluginLoader.InfoLog(message);
        }

        private static void ErrorLog(string message)
        {
            PluginLoader.ErrorLog(message);
        }
    }
}
