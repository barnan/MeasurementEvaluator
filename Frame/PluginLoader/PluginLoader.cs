using Frame.ConfigHandler;
using Frame.PluginLoader.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Frame.PluginLoader
{
    public class PluginLoader
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;



        private static ICollection<FactoryElement> _factories;
        private static ILogger _logger;
        private static object _lockObj = new object();
        private static ComponentList _componentList;
        private const string _COMPONENT_SECTION_NAME = "ComponentList";
        private const string _COMPONENT_FILE_NAME = "ComponentList";
        private const string _CONFIG_FILE_EXTENSION = ".config";

        private readonly IList<KeyValuePair<Type, Assembly>> _iRunables;

        public static string ConfigurationFolder { get; private set; }
        public static string CurrentExeFolder { get; private set; }
        public static string SpecificationFolder { get; private set; }
        public static string ReferenceFolder { get; private set; }
        public static string MeasurementDataFolder { get; private set; }
        public static string PluginsFolder { get; private set; }
        public static string ResultFolder { get; private set; }
        public static ConfigManager ConfigManager { get; private set; }

        public bool Initialized { get; private set; }


        static PluginLoader()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public PluginLoader()
        {
            _iRunables = new List<KeyValuePair<Type, Assembly>>();
        }


        public static string SendToInfoLogAndConsole(string message)
        {
            _logger?.Info(message);
            Console.WriteLine(message + Environment.NewLine);
            return message;
        }

        public static string SendToErrorLogAndConsole(string message)
        {
            _logger?.Error(message);
            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message + Environment.NewLine);
            Console.ForegroundColor = cc;
            return message;
        }


        /// <summary>
        /// Sets the used pluginfolder to the given path
        /// </summary>
        /// ///
        /// <param name="configurationFolder"></param>
        /// <param name="currentExeFolder"></param>
        /// <param name="pluginsFolder"></param>
        /// <param name="specificationFolder"></param>
        /// <param name="referenceFolder"></param>
        /// <param name="measDataFolder"></param>
        /// <param name="resultFolder"></param>
        /// <returns>if the path is a valid usable folder path -> true, otherwise -> false</returns>
        public bool Inititialize(string configurationFolder, string currentExeFolder, string pluginsFolder, string specificationFolder, string referenceFolder, string measDataFolder, string resultFolder)
        {
            try
            {
                lock (_lockObj)
                {
                    if (!IsPathFolder(configurationFolder) || !Directory.Exists(configurationFolder))
                    {
                        return false;
                    }
                    ConfigurationFolder = CheckDirectoryPath(configurationFolder);

                    if (!IsPathFolder(currentExeFolder) || !Directory.Exists(currentExeFolder))
                    {
                        return false;
                    }
                    CurrentExeFolder = CheckDirectoryPath(currentExeFolder);

                    if (!IsPathFolder(pluginsFolder) || !Directory.Exists(pluginsFolder))
                    {
                        return false;
                    }
                    PluginsFolder = CheckDirectoryPath(pluginsFolder);

                    if (!IsPathFolder(specificationFolder) || !Directory.Exists(specificationFolder))
                    {
                        return false;
                    }
                    SpecificationFolder = CheckDirectoryPath(specificationFolder);

                    if (!IsPathFolder(referenceFolder) || !Directory.Exists(referenceFolder))
                    {
                        return false;
                    }
                    ReferenceFolder = CheckDirectoryPath(referenceFolder);

                    if (!IsPathFolder(measDataFolder) || !Directory.Exists(measDataFolder))
                    {
                        return false;
                    }
                    MeasurementDataFolder = CheckDirectoryPath(measDataFolder);

                    if (!IsPathFolder(resultFolder) || !Directory.Exists(resultFolder))
                    {
                        return false;
                    }
                    ResultFolder = CheckDirectoryPath(resultFolder);

                    ConfigManager = new ConfigManager(ConfigurationFolder);

                    if (!LoadPlugins())
                    {
                        return Initialized = false;
                    }

#if RELEASE
                ShowWindow(GetConsoleWindow(), SW_HIDE);
#endif

                    return Initialized = true;
                }
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
        public bool Start()
        {
            lock (_lockObj)
            {
                if (!Initialized)
                {
                    _logger.Error($"{nameof(PluginLoader)} not initialized yet.");
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

                try
                {
                    Type type = _iRunables[0].Key;

                    IRunable runable = (IRunable)Activator.CreateInstance(type);

                    _logger.Info($"{nameof(IRunable)} was created with the type: {type}");

                    runable.Run();

                    _logger.Info($"{type} started.");

                    return true;
                }
                catch (Exception ex)
                {
                    SendToErrorLogAndConsole($"Exception occured: {ex}");
                    return false;
                }
            }
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


        #region private

        /// <summary>
        /// Load the available factories from the assemblies in the given folder
        /// </summary>
        /// <returns>Collection of factories</returns>
        private bool LoadPlugins()
        {
            _componentList = LoadComponentList();

            if (!Directory.Exists(PluginsFolder))
            {
                return false;
            }
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

                    SendToInfoLogAndConsole($"{assembly.FullName} assembly loadded.");
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
                throw new Exception($"No {nameof(IRunable)} comopnent was found in {PluginsFolder}");
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
                XElement componentListSection = ConfigManager.LoadSectionXElementFromFile(componentListFileName, _COMPONENT_SECTION_NAME, typeof(ComponentList));

                if (componentListSection == null)
                {
                    componentListSection = ConfigManager.CreateSectionXElement(_COMPONENT_SECTION_NAME, typeof(ComponentList));
                }

                if (!componentList.Load(componentListSection, _logger))
                {
                    ConfigManager.Save(componentListFileName, _COMPONENT_SECTION_NAME, componentListSection, typeof(ComponentList));
                }

                return componentList;
            }
            catch (Exception ex)
            {
                // todo throw stacktrace
                throw new Exception($"Problem during component list loading: {ex.Message}");
            }
        }

        #endregion


        internal class FactoryElement
        {
            internal IPluginFactory Factory { get; set; }
            internal string AssemblyName { get; set; }
        }


    }
}
