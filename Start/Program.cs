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
            catch (Exception)
            {
                Console.WriteLine("Logger could not been created.");
                return;
            }

            try
            {
                Process currentprocess = Process.GetCurrentProcess();
                SendToInfoLogAndConsole($"Process {currentprocess} started.");

                _currentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                SendToInfoLogAndConsole($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                SendToInfoLogAndConsole($"Application runtime folder: {_currentExeFolder}");

                // frame start:
                PluginLoader pluginLoader = new PluginLoader();
                if (!pluginLoader.Inititialize(_currentExeFolder))
                {
                    SendToErrorLogAndConsole("Frame initialization was NOT successful.");
                    return;
                }
                pluginLoader.Start();

                SendToInfoLogAndConsole($"Process {currentprocess} ended.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }
        }



        private static void SendToInfoLogAndConsole(string message)
        {
            PluginLoader.SendToInfoLogAndConsole(message);
        }

        private static void SendToErrorLogAndConsole(string message)
        {
            PluginLoader.SendToErrorLogAndConsole(message);
        }
    }
}
