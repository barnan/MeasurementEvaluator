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

        private static string SpecificationFolder { get; set; }
        private static string ReferenceFolderPath { get; set; }
        private static string MeasurementDataFolder { get; set; }
        private static string ResultFolder { get; set; }
        private static string PluginsFolder { get; set; }
        private static string CurrentExeFolder { get; set; }


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
                CurrentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                SendToInfoLogAndConsole($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                SendToInfoLogAndConsole($"Application runtime folder: {CurrentExeFolder}");

                // read some data from App settings
                if (!ReadConfig())
                {
                    return;
                }

                // frame start:
                PluginLoader pluginLoader = new PluginLoader();
                if (!pluginLoader.SetFolders(CurrentExeFolder, PluginsFolder, SpecificationFolder, ReferenceFolderPath, MeasurementDataFolder, ResultFolder))
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

                // specifications:
                if (settings[FolderSettingsKeys.SpecificactionFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.SpecificactionFolderKey, @".\Configuration\Specifications");
                }
                SpecificationFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.SpecificactionFolderKey], nameof(SpecificationFolder));
                if (SpecificationFolder == null)
                {
                    return false;
                }

                // references:
                if (settings[FolderSettingsKeys.ReferenceFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ReferenceFolderKey, @".\Configuration\References");
                }
                ReferenceFolderPath = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.ReferenceFolderKey], nameof(ReferenceFolderPath));
                if (ReferenceFolderPath == null)
                {
                    return false;
                }

                // meas data:
                if (settings[FolderSettingsKeys.MeasurementDataFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.MeasurementDataFolderKey, @".\Configuration\Measurements");
                }
                MeasurementDataFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.MeasurementDataFolderKey], nameof(MeasurementDataFolder));
                if (MeasurementDataFolder == null)
                {
                    return false;
                }

                // result:
                if (settings[FolderSettingsKeys.ResultFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ResultFolderKey, @".\Results");
                }
                ResultFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.ResultFolderKey], nameof(ResultFolder));
                if (MeasurementDataFolder == null)
                {
                    return false;
                }

                // plugins:
                if (settings[FolderSettingsKeys.PluginsFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.PluginsFolderKey, @".\Plugins");
                }
                PluginsFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.PluginsFolderKey], nameof(PluginsFolder));
                if (PluginsFolder == null)
                {
                    return false;
                }

                if (_logger.IsTraceEnabled)
                {
                    SendToInfoLogAndConsole($"{nameof(SpecificationFolder)}: {SpecificationFolder}");
                    SendToInfoLogAndConsole($"{nameof(ReferenceFolderPath)}: {ReferenceFolderPath}");
                    SendToInfoLogAndConsole($"{nameof(MeasurementDataFolder)}: {MeasurementDataFolder}");
                    SendToInfoLogAndConsole($"{nameof(ResultFolder)}: {ResultFolder}");
                    SendToInfoLogAndConsole($"{nameof(PluginsFolder)}: {PluginsFolder}");
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
            internal const string SpecificactionFolderKey = "SpecificationFolder";
            internal const string ReferenceFolderKey = "ReferenceFolder";
            internal const string MeasurementDataFolderKey = "MeasurementFolder";
            internal const string PluginsFolderKey = "PluginsFolder";
            internal const string ResultFolderKey = "ResultFolder";

        }

    }
}
