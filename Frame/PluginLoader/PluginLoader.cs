using Frame.ConfigHandler;
using Frame.PluginLoader.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Frame.PluginLoader
{
    public class PluginLoader
    {
        private static ICollection<IPluginFactory> _factories;
        private static ILogger _logger;
        private static object _lockObj = new object();

        private IList<KeyValuePair<Type, Assembly>> _iRunables;

        public static string ConfigurationFolder { get; private set; }
        public static string CurrentExeFolder { get; private set; }
        public static string SpecificationFolder { get; private set; }
        public static string ReferenceFolder { get; private set; }
        public static string MeasurementDataFolder { get; private set; }
        public static string PluginsFolder { get; private set; }
        public static string ResultFolder { get; private set; }
        public static ConfigManager ConfigManager { get; private set; }


        public PluginLoader()
        {
            _logger = LogManager.GetCurrentClassLogger();
            _iRunables = new List<KeyValuePair<Type, Assembly>>();
        }


        /// <summary>
        /// Sets the used pluginfolder to the given path
        /// </summary>
        /// /// <param name="currentExeFolder"></param>
        /// <param name="pluginsFolder"></param>
        /// <param name="specificationFolder"></param>
        /// <param name="referenceFolder"></param>
        /// <param name="measDataFolder"></param>
        /// <param name="resultFolder"></param>
        /// <returns>if the path is a valid usable folder path -> true, otherwise -> false</returns>
        public bool SetFolders(string configurationFolder, string currentExeFolder, string pluginsFolder, string specificationFolder, string referenceFolder, string measDataFolder, string resultFolder)
        {
            lock (_lockObj)
            {
                if (!IsPathFolder(configurationFolder))
                {
                    return false;
                }
                ConfigurationFolder = configurationFolder;

                if (!IsPathFolder(currentExeFolder))
                {
                    return false;
                }
                CurrentExeFolder = currentExeFolder;

                if (!IsPathFolder(pluginsFolder))
                {
                    return false;
                }
                PluginsFolder = pluginsFolder;

                if (!IsPathFolder(specificationFolder))
                {
                    return false;
                }
                SpecificationFolder = specificationFolder;

                if (!IsPathFolder(referenceFolder))
                {
                    return false;
                }
                ReferenceFolder = referenceFolder;

                if (!IsPathFolder(measDataFolder))
                {
                    return false;
                }
                MeasurementDataFolder = measDataFolder;

                if (!IsPathFolder(resultFolder))
                {
                    return false;
                }
                ResultFolder = resultFolder;

                ConfigManager = new ConfigManager(ConfigurationFolder);

                return LoadPlugins();
            }
        }

        public bool Start()
        {
            lock (_lockObj)
            {
                if (_iRunables.Count == 0)
                {
                    _logger.Error($"No {nameof(IRunable)} was found in folder: {PluginsFolder}");
                    return false;
                }

                if (_iRunables.Count > 1)
                {
                    _logger.Error($"More {nameof(IRunable)} was found in folder: {PluginsFolder}");

                    foreach (KeyValuePair<Type, Assembly> item in _iRunables)
                    {
                        _logger.Info($"{nameof(IRunable)} was found in type {item.Key} in assembly: {item.Value}");
                    }

                    return false;
                }

                Type type = _iRunables[0].Key;
                IRunable runable = (IRunable)Activator.CreateInstance(type);

                _logger.Info($"{nameof(IRunable)} was created with the type: {type}");

                runable.Run();

                _logger.Info($"{nameof(IRunable)} started.");

                return true;
            }
        }


        /// <summary>
        /// creates the component with the given title
        /// </summary>
        /// <typeparam name="T">type of the required component</typeparam>
        /// <param name="title">title of the required component</param>
        /// <returns>returns the instantiated component</returns>
        public static T CreateInstance<T>(string title)
        {
            lock (_lockObj)
            {
                if (_factories == null)
                {
                    _logger.Error("Factories are not created yet.");
                    return default(T);
                }

                List<T> instances = new List<T>();

                foreach (var factory in _factories)
                {
                    T instance = (T)factory.Create(typeof(T), title);

                    if (instance == null)
                    {
                        continue;
                    }

                    instances.Add(instance);
                }

                if (instances.Count == 0)
                {
                    _logger.Error($"No factory was found to create: {typeof(T)}");
                    return default(T);
                }

                if (instances.Count > 1)
                {
                    _logger.Error($"More than one factories was found to create: {typeof(T)}");
                    return default(T);
                }

                return instances[0];
            }
        }


        #region private

        /// <summary>
        /// Load the available factories from the assemblies in the given folder
        /// </summary>
        /// <returns>Collection of factories</returns>
        private bool LoadPlugins()
        {
            if (Directory.Exists(PluginsFolder))
            {
                // gather all dll names:
                string[] dllFileNames = Directory.GetFiles(PluginsFolder, "*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    try
                    {
                        // load the assembly:
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        Assembly assembly = Assembly.Load(an);
                        assemblies.Add(assembly);

                        _logger.Info($"{assembly.FullName} assembly loadded.");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Could not load assembyly from file: {dllFile} -> {ex}");
                    }
                }

                // go through all assemblies and look for IRunable component:

                foreach (Assembly assembly in assemblies)
                {
                    try
                    {
                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }

                            if (typeof(IRunable).IsAssignableFrom(type))
                            {
                                _iRunables.Add(new KeyValuePair<Type, Assembly>(type, assembly));
                                _logger.Info($"{nameof(IRunable)} found in {assembly.FullName} -> {type}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Exception occured during assembly investigation: {assembly.FullName} -> {ex}");
                    }
                }

                // go through all assemblies and and check whether they implement IPluginFactory interface:

                ICollection<IPluginFactory> factories = new List<IPluginFactory>();
                foreach (Assembly assembly in assemblies)
                {
                    try
                    {
                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }

                            if (typeof(IPluginFactory).IsAssignableFrom(type))
                            {
                                factories.Add((IPluginFactory)Activator.CreateInstance(type));
                                _logger.Info($"{type} added to plugin factories.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Could not load factory from assembly: {assembly.FullName} -> {ex}");
                    }
                }
                _factories = factories;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether the received path string contains folder path or not
        /// </summary>
        /// <param name="path">the investigated path string</param>
        /// <returns>is it directory path or not</returns>
        private bool IsPathFolder(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
            {
                _logger.Error("Arrived path is null, empty or whitespace.");
                return false;
            }

            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
            }

            return false;
        }

        #endregion
    }
}
