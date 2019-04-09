using Frame.ConfigHandler;
using Frame.PluginLoader.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

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
        private readonly string _componentSectionName = "ComponentList";

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

        public static string SendToErrrorLogAndConsole(string message)
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
                SendToErrrorLogAndConsole($"Problem with input variable: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                SendToErrrorLogAndConsole($"Exception occured: {ex.Message}");
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
                    SendToErrrorLogAndConsole($"Exception occured: {ex}");
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

            if (_componentList.Components.All(p => p.Name != name) || !_componentList.Components.Where(p => p.Name == name).Any(p => p.Interfaces.Contains(interfaceType.Name)))
            {
                _logger.Error($"ComponentList does not contain {name} {interfaceType}");
                return null;
            }

            var component = _componentList.Components.Where(p => p.Name == name).ToList();
            if (component.Count != 1)
            {
                _logger.Error($"Ambiguity between components in the ComponentList. {name} appears more times.");
                return null;
            }

            ICollection<FactoryElement> fact = _factories.Where(p => p.AssemblyName == component[0].AssemblyName).ToList();

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
                _logger.Error($"No factory was found to create: {interfaceType}");
                return null;
            }

            if (instances.Count > 1)
            {
                _logger.Error($"More than one factories was found to create: {interfaceType}");
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
                    SendToErrrorLogAndConsole($"Could not load assemby from file: {dllFile} -> {ex}");
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
                    SendToErrrorLogAndConsole($"Exception occured during assembly investigation: {assembly.FullName} -> {ex}");
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
                    SendToErrrorLogAndConsole($"Could not load factory from assembly: {assembly.FullName} -> {ex}");
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
                throw new ArgumentNullException("Arrived path is null, empty or whitespace.");
            }

            FileAttributes attr = File.GetAttributes(path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Reads the list of available components from the ComponentList.config or creates a dummy component list
        /// </summary>
        /// <returns>returns with the componentlist from the ComponentList.config or a dummy componentList (example)</returns>
        private ComponentList LoadComponentList()
        {
            try
            {
                string componentListFileName = Path.Combine(ConfigurationFolder, "ComponentList.config");

                ConfigManager.CreateConfigFileIfNotExisting(componentListFileName);

                ComponentList componentList = new ComponentList();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement componentListSection = ConfigManager.LoadXmlElement(xmlDoc, componentListFileName, _componentSectionName);

                if (componentListSection == null)
                {
                    componentListSection = ConfigManager.CreateXmlSection(xmlDoc, _componentSectionName, typeof(ComponentList));
                }

                if (!componentList.Load(xmlDoc, componentListSection))
                {
                    ConfigManager.Save(componentListFileName, "ComponentList", componentListSection, typeof(ComponentList));
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
