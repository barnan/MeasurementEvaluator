using MyFrameWork.Interfaces;
using MyFrameWork.Loggers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyFrameWork.PluginLoading
{
    public class PluginLoader
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        private static ICollection<FactoryElement> _factories;
        private static IMyLogger _logger;
        private static IMyLogger _consoleLogger;
        private readonly object _lockObj = new object();
        private static ComponentList _componentList;

        private readonly IList<KeyValuePair<Type, Assembly>> _iRunables;

        public static string ConfigurationFolder { get; private set; }
        public static string CurrentExeFolder { get; private set; }
        public static string SpecificationFolder { get; private set; }
        public static string ReferenceFolder { get; private set; }
        public static string MeasurementDataFolder { get; private set; }
        public static string PluginsFolder { get; private set; }
        public static string ResultFolder { get; private set; }
        public static ConfigHandler.ConfigHandler ConfigHandler { get; private set; }

        public static bool Initialized { get; private set; }

        //public static IUIMessageControl MessageControll { get; private set; }

        #region static

        static PluginLoader()
        {
            _consoleLogger = GetConsoleLogger();
        }

        internal static IMyLogger GetConsoleLogger([CallerMemberName] string desiredName = null)
        {
            string name = desiredName;

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                name = new StackFrame(1, false).GetMethod().DeclaringType.Name;     // to get the caller class name
            }

            return new LoggerToConsole(name);
        }

        internal static string SendToErrorLogAndConsole(string message)
        {
            _consoleLogger?.Error(message);
            return message;
        }

        public static IMyLogger GetLogger(string desiredName = null)
        {
            string name = desiredName;

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                name = new StackFrame(1, false).GetMethod().DeclaringType.Name;
            }

            return new LoggerToNLog(name);
        }

        /// <summary>
        /// creates the component with the given title
        /// </summary>
        /// <typeparam name="T">type of the required component</typeparam>
        /// <param name="name">title of the required component</param>
        /// <returns>returns the instantiated component</returns>
        public static T CreateInstance<T>(string name)
        {
            return (T)CreateInstance(typeof(T), name);
        }

        private static object CreateInstance(Type interfaceType, string name)
        {
            if (_factories == null)
            {
                _logger.Error("Factories are not created yet.");
                return null;
            }

            var components = _componentList.Components.Where(p => p.Name == name).ToList();
            if (components.Count > 1)
            {
                _logger.Error($"Ambiguity between components. {name} appears more than once in the ComponentList.");
                return null;
            }
            if (components.Count < 1)
            {
                _logger.Error($"No component was found with the given name:{name} in the ComponentList.");
                return null;
            }

            //if (components[0].Interfaces.All(p => !(interfaceType.IsAssignableFrom(Type.GetType(p)))))
            //{
            //    _logger.Error($"The found compnent (with name: {name}) does not implement the given interface: {nameof(interfaceType)}. The implemented interfaces: {string.Join(",", components[0].Interfaces)}");
            //    return null;
            //}

            ICollection<FactoryElement> fact = _factories.Where(p => p.AssemblyName == components[0].AssemblyName).ToList();

            List<object> instances = new List<object>();
            foreach (var factory in fact)
            {
                object instance = factory.Factory.Create(interfaceType, name);
                if (instance == null)
                {
                    continue;
                }

                instances.Add(instance);
            }

            if (instances.Count == 0)
            {
                SendToErrorLogAndConsole($"No factory was found to create: {name}({interfaceType})");
                return null;
            }

            if (instances.Count > 1)
            {
                SendToErrorLogAndConsole($"More than one factories was found to create: {name}({interfaceType})");
                return null;
            }

            return instances[0];
        }

        #endregion

        #region ctor

        public PluginLoader()
        {
            _iRunables = new List<KeyValuePair<Type, Assembly>>();
        }

        #endregion

        #region public

        /// <summary>
        /// Sets the used pluginfolder to the given path
        /// </summary>
        /// ///
        /// <param name="folder"></param>
        /// <returns>if the path is a valid usable folder path -> true, otherwise -> false</returns>
        public bool Inititialize(string folder)
        {
            try
            {
                var settings = ConfigurationManager.AppSettings;

                lock (_lockObj)
                {
                    if (!IsPathFolder(folder) || !Directory.Exists(folder))
                    {
                        return false;
                    }
                    CurrentExeFolder = CheckDirectoryPath(folder);

                    // Configuration folder:
                    if (settings[FolderSettingsKeys.ConfigurationFolderKey] == null)
                    {
                        settings.Add(FolderSettingsKeys.ConfigurationFolderKey, @".\Configuration\");
                    }
                    ConfigurationFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.ConfigurationFolderKey], nameof(ConfigurationFolder));
                    if (!IsPathFolder(ConfigurationFolder) || !Directory.Exists(ConfigurationFolder))
                    {
                        return false;
                    }

                    // Plugins folder:
                    if (settings[FolderSettingsKeys.PluginsFolderKey] == null)
                    {
                        settings.Add(FolderSettingsKeys.PluginsFolderKey, @".\Plugins\");
                    }
                    PluginsFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.PluginsFolderKey], nameof(PluginsFolder));
                    if (!IsPathFolder(PluginsFolder) || !Directory.Exists(PluginsFolder))
                    {
                        return false;
                    }

                    // Specification folder:
                    if (settings[FolderSettingsKeys.SpecificactionFolderKey] == null)
                    {
                        settings.Add(FolderSettingsKeys.SpecificactionFolderKey, @".\Configuration\Specifications\");
                    }
                    SpecificationFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.SpecificactionFolderKey], nameof(SpecificationFolder));
                    if (!IsPathFolder(SpecificationFolder) || !Directory.Exists(SpecificationFolder))
                    {
                        return false;
                    }

                    // Reference folder:
                    if (settings[FolderSettingsKeys.ReferenceFolderKey] == null)
                    {
                        settings.Add(FolderSettingsKeys.ReferenceFolderKey, @".\Configuration\References\");
                    }
                    ReferenceFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.ReferenceFolderKey], nameof(ReferenceFolder));
                    if (!IsPathFolder(ReferenceFolder) || !Directory.Exists(ReferenceFolder))
                    {
                        return false;
                    }

                    // measurement data:
                    if (settings[FolderSettingsKeys.MeasurementDataFolderKey] == null)
                    {
                        settings.Add(FolderSettingsKeys.MeasurementDataFolderKey, @".\Configuration\Measurements\");
                    }
                    MeasurementDataFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.MeasurementDataFolderKey], nameof(MeasurementDataFolder));
                    if (!IsPathFolder(MeasurementDataFolder) || !Directory.Exists(MeasurementDataFolder))
                    {
                        return false;
                    }

                    // Result folder:
                    if (settings[FolderSettingsKeys.ResultFolderKey] == null)
                    {
                        settings.Add(FolderSettingsKeys.ResultFolderKey, @".\Results\");
                    }
                    ResultFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.ResultFolderKey], nameof(ResultFolder));
                    if (!IsPathFolder(ResultFolder) || !Directory.Exists(ResultFolder))
                    {
                        return false;
                    }

                    ConfigHandler = new ConfigHandler.ConfigHandler(ConfigurationFolder);

                    if (!LoadPlugins(PluginsFolder))
                    {
                        return Initialized = false;
                    }

                    _logger = GetLogger();

                    //MessageControll = new UIMessageControl();

                    return Initialized = true;
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                _logger.Error($"Problem during configuration folder loading from App settings: {ex.Message}");
                return false;
            }
            catch (ArgumentNullException ex)
            {
                SendToErrorLogAndConsole($"Problem with input variable: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                SendToErrorLogAndConsole($"Exception occured: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Starts the available IRunables from the [PluginsFolder] folder
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            lock (_lockObj)
            {
                // first initialize if it wasnt't:
                if (!Initialized && !Inititialize(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName))
                {
                    _logger.Error($"{nameof(PluginLoader)} not initialized yet.");
                    return;
                }

                // log out all irunables
                if (_iRunables.Count > 1)
                {
                    _logger.Error($"More {nameof(IRunable)} was found in folder: {PluginsFolder}");

                    foreach (KeyValuePair<Type, Assembly> item in _iRunables)
                    {
                        _logger.Info($"{nameof(IRunable)} was found in type {item.Key} in assembly: {item.Value}");
                    }

                    return;
                }

                try
                {
                    // try to start all irunnables:
                    foreach (KeyValuePair<Type, Assembly> iRunable in _iRunables)
                    {
                        Type type = iRunable.Key;
                        IRunable runable = (IRunable)Activator.CreateInstance(type);

                        _logger.Info($"{nameof(IRunable)} was created with the type: {type}");

                        runable.Run();

                        _logger.Info($"{type} started.");
                    }
                }
                catch (Exception ex)
                {
                    SendToErrorLogAndConsole($"Exception occured: {ex}");
                }
            }
        }

        #endregion

        #region private  

        /// <summary>
        /// Load the available factories from the assemblies in the given folder
        /// </summary>
        /// <returns>Collection of factories</returns>
        private bool LoadPlugins(string pluginFolder)
        {
            _componentList = ComponentList.CreateComponentList();

            if (!Directory.Exists(pluginFolder))
            {
                return false;
            }
            // gather all dll names:
            string[] dllFileNames = Directory.GetFiles(pluginFolder, "*.dll");

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                try
                {
                    // load the assembly:
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(an);
                    assemblies.Add(assembly);

                    SendToInfoLogAndConsole($"{assembly.FullName} assembly loadded nad added to the list.");
                }
                catch (Exception ex)
                {
                    SendToErrorLogAndConsole($"Could not load assemby from file: {dllFile} -> {ex}");
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
                            SendToInfoLogAndConsole($"{nameof(IRunable)} found in {assembly.FullName} -> {type}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    SendToErrorLogAndConsole($"Exception occured during assembly investigation: {assembly.FullName} -> {ex}");
                }
            }

            if (_iRunables.Count == 0)
            {
                throw new Exception($"No {nameof(IRunable)} comopnent was found in {pluginFolder}");
            }

            // go through all assemblies and and check whether they implement IPluginFactory interface:

            ICollection<FactoryElement> factories = new List<FactoryElement>();
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
                            string assemblyName = assembly.FullName.Split(',')[0];

                            factories.Add(new FactoryElement { Factory = (IPluginFactory)Activator.CreateInstance(type), AssemblyName = assemblyName });
                            SendToInfoLogAndConsole($"{type} added to plugin factories.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    SendToErrorLogAndConsole($"Could not load factory from assembly: {assembly.FullName} -> {ex}");
                }
            }
            _factories = factories;
            return true;
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
                throw new ArgumentNullException("Arrived path is null, empty or consists of whitespace.");
            }

            FileAttributes attr = File.GetAttributes(path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }

            return false;
        }


        private string CheckDirectoryPath(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (dirInfo.FullName != path)
            {
                return dirInfo.FullName;
            }

            return path;
        }


        private string CreateFinalPath(string currentExeFolder, string specialFolder, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ConfigurationErrorsException(PluginLoader.SendToErrorLogAndConsole("Received path-name is null."));
            }

            if (string.IsNullOrEmpty(specialFolder))
            {
                throw new ConfigurationErrorsException(PluginLoader.SendToErrorLogAndConsole($"{name} is null."));
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
            return CheckDirectoryPath(combinedPath);
        }


        [Conditional("RELEASE")]
        private void HideConsole()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }


        #endregion

    }
}
