using Frame.PluginLoader;
using NLog;
using System;
using System.Configuration;
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
                _currentExeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                PluginLoader.SendToInfoLogAndConsole($"Application started: {Assembly.GetExecutingAssembly().FullName}");
                PluginLoader.SendToInfoLogAndConsole($"Application runtime folder: {_currentExeFolder}");

                // read some data from App settings
                if (!ReadConfig())
                {
                    return;
                }

                // frame start:
                PluginLoader pluginLoader = new PluginLoader();
                if (!pluginLoader.Inititialize(
                    _configurationFolder,
                    _currentExeFolder,
                    _pluginsFolder,
                    _specificationFolder,
                    _referenceFolderPath,
                    _measurementDataFolder,
                    _resultFolder))
                {
                    PluginLoader.SendToErrrorLogAndConsole("Frame initialization was NOT successful.");
                    return;
                }
                pluginLoader.Start();

                PluginLoader.SendToInfoLogAndConsole("Frame started successfully.");
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
                var settings = ConfigurationManager.AppSettings;

                // cnfigurations:
                if (settings[FolderSettingsKeys.ConfigurationFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ConfigurationFolderKey, @".\Configuration");
                }
                _configurationFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.ConfigurationFolderKey], nameof(_configurationFolder));

                // specifications:
                if (settings[FolderSettingsKeys.SpecificactionFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.SpecificactionFolderKey, @".\Configuration\Specifications");
                }
                _specificationFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.SpecificactionFolderKey], nameof(_specificationFolder));

                // references:
                if (settings[FolderSettingsKeys.ReferenceFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ReferenceFolderKey, @".\Configuration\References");
                }
                _referenceFolderPath = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.ReferenceFolderKey], nameof(_referenceFolderPath));

                // meas data:
                if (settings[FolderSettingsKeys.MeasurementDataFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.MeasurementDataFolderKey, @".\Configuration\Measurements");
                }
                _measurementDataFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.MeasurementDataFolderKey], nameof(_measurementDataFolder));

                // result:
                if (settings[FolderSettingsKeys.ResultFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.ResultFolderKey, @".\Results");
                }
                _resultFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.ResultFolderKey], nameof(_resultFolder));

                // plugins:
                if (settings[FolderSettingsKeys.PluginsFolderKey] == null)
                {
                    settings.Add(FolderSettingsKeys.PluginsFolderKey, @".\Plugins");
                }
                _pluginsFolder = CreateFinalPath(_currentExeFolder, settings[FolderSettingsKeys.PluginsFolderKey], nameof(_pluginsFolder));

                if (_logger.IsTraceEnabled)
                {
                    PluginLoader.SendToInfoLogAndConsole($"{nameof(_specificationFolder)}: {_specificationFolder}");
                    PluginLoader.SendToInfoLogAndConsole($"{nameof(_referenceFolderPath)}: {_referenceFolderPath}");
                    PluginLoader.SendToInfoLogAndConsole($"{nameof(_measurementDataFolder)}: {_measurementDataFolder}");
                    PluginLoader.SendToInfoLogAndConsole($"{nameof(_resultFolder)}: {_resultFolder}");
                    PluginLoader.SendToInfoLogAndConsole($"{nameof(_pluginsFolder)}: {_pluginsFolder}");
                }

                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                _logger.Error($"Problem during configuration folder loading from App settings: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error($"Exceptin occured: {ex.Message}");
                return false;
            }
        }

        private static string CreateFinalPath(string currentExeFolder, string specialFolder, string name)
        {
            if (string.IsNullOrEmpty(currentExeFolder) || string.IsNullOrEmpty(name))
            {
                throw new ConfigurationErrorsException(PluginLoader.SendToErrrorLogAndConsole("Received exefolder-path OR path-name is null."));
            }

            if (string.IsNullOrEmpty(specialFolder))
            {
                throw new ConfigurationErrorsException(PluginLoader.SendToErrrorLogAndConsole($"{name} is null."));
            }

            if (Path.IsPathRooted(specialFolder))
            {
                if (!Directory.Exists(specialFolder))
                {
                    Directory.CreateDirectory(specialFolder);
                    PluginLoader.SendToInfoLogAndConsole($"{specialFolder} created.");
                }

                PluginLoader.SendToInfoLogAndConsole($"{specialFolder} will be used as {name}");
                return specialFolder;
            }

            string combinedPath = Path.Combine(currentExeFolder, specialFolder);

            if (!Directory.Exists(combinedPath))
            {
                Directory.CreateDirectory(combinedPath);
                PluginLoader.SendToInfoLogAndConsole($"Combined {name} directory ({combinedPath}) created.");
            }

            PluginLoader.SendToInfoLogAndConsole($"Combined {name} ({combinedPath}) will be used.");
            return combinedPath;
        }


        private static class FolderSettingsKeys
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
