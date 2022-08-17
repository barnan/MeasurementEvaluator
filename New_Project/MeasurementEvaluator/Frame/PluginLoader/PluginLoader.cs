using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Xml.Linq;
using Frame.Configuration;
using Frame.MessageHandler;
using FrameInterfaces;

namespace Frame.PluginLoader
{
    public class PluginLoader
    {
        private static ICollection<FactoryElement> _factories;
        private static object _lockObj = new object();
        private const string _COMPONENT_SECTION_NAME = "ComponentList";
        private const string _COMPONENT_FILE_NAME = "ComponentList";
        private const string _CONFIG_FILE_EXTENSION = ".config";

        private readonly List<KeyValuePair<Type, Assembly>> _iRunnables;
        private static ComponentList ComponentList { get; set; }
        public static string ConfigurationFolder { get; private set; }
        public static string CurrentExeFolder { get; private set; }
        public static string SpecificationFolder { get; private set; }
        public static string ReferenceFolder { get; private set; }
        public static string MeasurementDataFolder { get; private set; }
        public static string PluginsFolder { get; private set; }
        public static string ResultFolder { get; private set; }
        public static ConfigManager ConfigManager { get; private set; }
        private static IMyLogger Logger { get; set; }
        public static IUIMessageControl MessageControll { get; private set; }

        public static bool Initialized { get; private set; }


        static PluginLoader()
        {
            try
            {
                Logger = StandardLogger.StandardLogger.GetLogger(nameof(PluginLoader));
            }
            catch (Exception ex)
            {
                // WTF????
            }
        }

        public PluginLoader()
        {
            _iRunnables = new List<KeyValuePair<Type, Assembly>>();
        }
        

        /// <summary>
        /// Sets the used pluginfolder to the given path
        /// </summary>
        /// <param name="currentExeFolder"></param>
        /// <returns>if the path is a valid usable folder path -> true, otherwise -> false</returns>
        public bool Initialize(string currentExeFolder)
        {
            try
            {
                NameValueCollection settings = ConfigurationManager.AppSettings;

                lock (_lockObj)
                {
                    if (!IsPathFolder(currentExeFolder) || !Directory.Exists(currentExeFolder))
                    {
                        return false;
                    }
                    CurrentExeFolder = CheckDirectoryPath(currentExeFolder);

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
                    if (settings[FolderSettingsKeys.SpecificationFolderKey] == null)
                    {
                        settings.Add(FolderSettingsKeys.SpecificationFolderKey, @".\Configuration\Specifications\");
                    }
                    SpecificationFolder = CreateFinalPath(CurrentExeFolder, settings[FolderSettingsKeys.SpecificationFolderKey], nameof(SpecificationFolder));
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

                    ConfigManager = new ConfigManager(ConfigurationFolder);

                    ComponentList = LoadComponentList();

                    if (!GatherPlugins())
                    {
                        return Initialized = false;
                    }

                    MessageControll = new UIMessageControl();

                    return Initialized = true;
                }
            }
            catch (ConfigurationException ex)
            {
                Logger.Error($"Problem during configuration folder loading from App settings: {ex.Message}");
                return false;
            }
            catch (ArgumentNullException ex)
            {
                Logger.Error($"Problem with input variable: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred during {nameof(Initialize)}: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Starts the available IRunnable from the [PluginsFolder] folder
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            lock (_lockObj)
            {
                if (!Initialized)
                {
                    Logger.Error($"{nameof(PluginLoader)} was not initialized yet");
                    return;
                }

                if (_iRunnables.Count > 1)
                {
                    Logger.Error($"More {nameof(IRunnable)} was found in folder: {PluginsFolder}");

                    foreach (KeyValuePair<Type, Assembly> item in _iRunnables)
                    {
                        Logger.Info($"{nameof(IRunnable)} was found in type {item.Key} in assembly: {item.Value}");
                    }

                    return;
                }

                try
                {
                    Type type = _iRunnables[0].Key;

                    IRunnable runnable = (IRunnable)Activator.CreateInstance(type);

                    Logger.Info($"{nameof(IRunnable)} was created with the type: {type}");

                    runnable.Run();

                    Logger.Info($"{type} started");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Exception occurred during {nameof(Start)}: {ex}");
                }
            }
        }


        private static object CreateInstance(Type interfaceType, string name)
        {
            if (_factories == null)
            {
                Logger.Error("Factories were not created yet");
                return null;
            }

            var components = ComponentList.Components.Where(p => p.Name == name).ToList();
            switch (components.Count)
            {
                case > 1:
                    Logger.Error($"Ambiguity between components. {name} appears more than once in the ComponentList");
                    return null;
                case < 1:
                    Logger.Error($"No component was found with the given name:{name} in the ComponentList");
                    return null;
            }

            //if (components[0].Interfaces.All(p => !(interfaceType.IsAssignableFrom(Type.GetType(p)))))
            //{
            //    ErrorLog($"The found component (with name: {name}) does not implement the given interface: {nameof(interfaceType)}. The implemented interfaces: {string.Join(",", components[0].Interfaces)}");
            //    return null;
            //}

            ICollection<FactoryElement> selectedFactories = _factories.Where(p => p.AssemblyName == components[0].AssemblyName).ToList();

            List<object> instances = new List<object>();
            foreach (var fact in selectedFactories)
            {
                object instance = fact.Factory.Create(interfaceType, name);
                if (instance == null)
                {
                    continue;
                }

                instances.Add(instance);
            }

            switch (instances.Count)
            {
                case 0:
                    Logger.Error($"No factory was found to create: {name}({interfaceType})");
                    return null;
                case > 1:
                    Logger.Error($"More than one factories was found to create: {name}({interfaceType})");
                    return null;
                default:
                    return instances[0];
            }
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

        public static IMyLogger GetLogger(string name)
        {
            return StandardLogger.StandardLogger.GetLogger(name);
        }

        #region private

        /// <summary>
        /// Load the available factories from the assemblies in the given folder
        /// </summary>
        /// <returns>Collection of factories</returns>
        private bool GatherPlugins()
        {
            // gather all dll names:
            string[] dllFileNames = Directory.GetFiles(PluginsFolder, "*.dll");

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFileName in dllFileNames)
            {
                try
                {
                    // load the assembly:
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFileName);
                    Assembly assembly = Assembly.Load(an);
                    assemblies.Add(assembly);

                    Logger.Info($"{assembly.FullName} assembly was loaded and added to the list");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Could not load assembly from file: {dllFileName} -> {ex}");
                }
            }

            // go through all assemblies and look for IRunnable component:

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

                        if (typeof(IRunnable).IsAssignableFrom(type))
                        {
                            _iRunnables.Add(new KeyValuePair<Type, Assembly>(type, assembly));
                            Logger.Info($"{nameof(IRunnable)} was found in {assembly.FullName} -> {type}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Exception occurred during assembly investigation: {assembly.FullName} -> {ex}");
                }
            }

            if (_iRunnables.Count == 0)
            {
                throw new Exception($"No {nameof(IRunnable)} component was found in {PluginsFolder}");
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
                            Logger.Info($"{type} added to plugin factories");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Could not load factory from assembly: {assembly.FullName} -> {ex}");
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
                throw new ArgumentNullException("Arrived path is null, empty or consists of whitespace");
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
                throw new Exception("Received path-name is null");
            }

            if (string.IsNullOrEmpty(specialFolder))
            {
                throw new Exception($"{name} is null");
            }

            // the given specialfolder is given with absolute path:
            if (Path.IsPathRooted(specialFolder))
            {
                if (!Directory.Exists(specialFolder))
                {
                    Directory.CreateDirectory(specialFolder);
                    Logger.Info($"{specialFolder} created");
                }

                Logger.Info($"{specialFolder} will be used as {name}");
                return specialFolder;
            }

            // the given specialfolder is not given as relative path:
            string combinedPath = Path.Combine(currentExeFolder, specialFolder);

            if (!Directory.Exists(combinedPath))
            {
                Directory.CreateDirectory(combinedPath);
                Logger.Info($"Combined directory ({combinedPath}) was created for {name}");
            }

            Logger.Info($"Combined ({combinedPath}) will be used as {name}");
            return CheckDirectoryPath(combinedPath);
        }

        #endregion

        #region internal classes

        private static class FolderSettingsKeys
        {
            internal const string ConfigurationFolderKey = "ConfigurationFolder";
            internal const string SpecificationFolderKey = "SpecificationFolder";
            internal const string ReferenceFolderKey = "ReferenceFolder";
            internal const string MeasurementDataFolderKey = "MeasurementFolder";
            internal const string PluginsFolderKey = "PluginsFolder";
            internal const string ResultFolderKey = "ResultFolder";
        }

        #endregion


        /// <summary>
        /// Reads the list of available components from the ComponentList.config or creates a dummy component list
        /// </summary>
        /// <returns>returns with the componentlist from the ComponentList.config or a dummy componentList (example)</returns>
        private ComponentList LoadComponentList()
        {
            try
            {
                string componentListFileName = Path.Combine(ConfigurationFolder, $"{_COMPONENT_FILE_NAME}{_CONFIG_FILE_EXTENSION}");

                ConfigManager.CreateConfigFileIfNotExisting(componentListFileName);

                ComponentList componentList = new ComponentList();
                XElement componentListSection = ConfigManager.LoadSectionFromFile(componentListFileName, _COMPONENT_SECTION_NAME);

                if (componentListSection == null)
                {
                    componentListSection = ConfigManager.CreateSection(_COMPONENT_SECTION_NAME, typeof(ComponentList).Assembly.ToString());
                }

                if (!componentList.Load(componentListSection, Logger))
                {
                    ConfigManager.Save(componentListFileName, _COMPONENT_SECTION_NAME, componentListSection);
                }

                return componentList;
            }
            catch (Exception ex)
            {
                // todo throw stacktrace
                throw new Exception($"Problem during component list loading: {ex.Message}");
            }
        }
        
    }
}
