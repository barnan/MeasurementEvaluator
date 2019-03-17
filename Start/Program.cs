using Frame.PluginLoader;
using NLog;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Start
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        private static ILogger _logger;
        private static ManualResetEvent _uiFinishedEvent = new ManualResetEvent(false);

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
                _currentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                SendToInfoLogAndConsole($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                SendToInfoLogAndConsole($"Application runtime folder: {_currentExeFolder}");

                // read some data from App settings
                if (!ReadConfig())
                {
                    return;
                }

                // frame start:
                PluginLoader pluginLoader = new PluginLoader();
                if (!pluginLoader.SetFolders(
                    _configurationFolder,
                    _currentExeFolder,
                    _pluginsFolder,
                    _specificationFolder,
                    _referenceFolderPath,
                    _measurementDataFolder,
                    _resultFolder))
                {
                    SendToErrrorLogAndConsole("Frame setup was not successful.");
                    return;
                }
                pluginLoader.Start();

                SendToInfoLogAndConsole("Frame started successfully.");

                ShowWindow(GetConsoleWindow(), SW_HIDE);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }
        }


        private static bool ReadConfig()
        {
            try
            {
                var settings = System.Configuration.ConfigurationManager.AppSettings;

                // cnfigurations:
                if (settings[FolderSettingsKeys.ConfigurationFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ConfigurationFolderKey, @".\Configuration");
                }
                _configurationFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.ConfigurationFolderKey], nameof(_configurationFolder));
                if (_configurationFolder == null)
                {
                    return false;
                }

                // specifications:
                if (settings[FolderSettingsKeys.SpecificactionFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.SpecificactionFolderKey, @".\Configuration\Specifications");
                }
                _specificationFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.SpecificactionFolderKey], nameof(_specificationFolder));
                if (_specificationFolder == null)
                {
                    return false;
                }

                // references:
                if (settings[FolderSettingsKeys.ReferenceFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ReferenceFolderKey, @".\Configuration\References");
                }
                _referenceFolderPath = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.ReferenceFolderKey], nameof(_referenceFolderPath));
                if (_referenceFolderPath == null)
                {
                    return false;
                }

                // meas data:
                if (settings[FolderSettingsKeys.MeasurementDataFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.MeasurementDataFolderKey, @".\Configuration\Measurements");
                }
                _measurementDataFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.MeasurementDataFolderKey], nameof(_measurementDataFolder));
                if (_measurementDataFolder == null)
                {
                    return false;
                }

                // result:
                if (settings[FolderSettingsKeys.ResultFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ResultFolderKey, @".\Results");
                }
                _resultFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.ResultFolderKey], nameof(_resultFolder));
                if (_measurementDataFolder == null)
                {
                    return false;
                }

                // plugins:
                if (settings[FolderSettingsKeys.PluginsFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.PluginsFolderKey, @".\Plugins");
                }
                _pluginsFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.PluginsFolderKey], nameof(_pluginsFolder));
                if (_pluginsFolder == null)
                {
                    return false;
                }

                if (_logger.IsTraceEnabled)
                {
                    SendToInfoLogAndConsole($"{nameof(_specificationFolder)}: {_specificationFolder}");
                    SendToInfoLogAndConsole($"{nameof(_referenceFolderPath)}: {_referenceFolderPath}");
                    SendToInfoLogAndConsole($"{nameof(_measurementDataFolder)}: {_measurementDataFolder}");
                    SendToInfoLogAndConsole($"{nameof(_resultFolder)}: {_resultFolder}");
                    SendToInfoLogAndConsole($"{nameof(_pluginsFolder)}: {_pluginsFolder}");
                }

                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                _logger.Error($"Problem during configuration folder loading from App settings: {ex.Message}");
                return false;
            }
        }

        private static string CreateFinalPath(string currentExeFolder, string specialFolder, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(currentExeFolder) || string.IsNullOrEmpty(name))
                {
                    SendToErrrorLogAndConsole("Received exefolder-path OR path-name is null.");
                    return null;
                }

                if (string.IsNullOrEmpty(specialFolder))
                {
                    SendToErrrorLogAndConsole($"{name} is null.");
                    return null;
                }

                if (Path.IsPathRooted(specialFolder))
                {
                    if (!Directory.Exists(specialFolder))
                    {
                        Directory.CreateDirectory(specialFolder);
                        SendToInfoLogAndConsole($"{specialFolder} created.");
                    }

                    SendToInfoLogAndConsole($"{name} ({specialFolder}) wil be used.");
                    return specialFolder;
                }

                string combinedPath = Path.Combine(currentExeFolder, specialFolder);

                if (!Directory.Exists(combinedPath))
                {
                    SendToInfoLogAndConsole($"Combined {name} ({combinedPath}) created.");
                    Directory.CreateDirectory(combinedPath);
                }

                SendToInfoLogAndConsole($"Combined {name} ({combinedPath}) will be used.");
                return combinedPath;
            }
            catch (Exception ex)
            {
                SendToErrrorLogAndConsole($"Problem during {name} check: {ex}");
                return null;
            }
        }

        private static void SendToInfoLogAndConsole(string message)
        {
            _logger.Info(message);
            Console.WriteLine(message + Environment.NewLine);
        }

        private static void SendToErrrorLogAndConsole(string message)
        {
            _logger.Error(message);
            Console.WriteLine(message + Environment.NewLine);
        }

        internal static class FolderSettingsKeys
        {
            internal const string ConfigurationFolderKey = "ConfigurationFolder";
            internal const string SpecificactionFolderKey = "SpecificationFolder";
            internal const string ReferenceFolderKey = "ReferenceFolder";
            internal const string MeasurementDataFolderKey = "MeasurementFolder";
            internal const string PluginsFolderKey = "PluginsFolder";
            internal const string ResultFolderKey = "ResultFolder";

        }

    }
}
