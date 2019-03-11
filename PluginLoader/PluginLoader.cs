using NLog;
using PluginLoading.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PluginLoading
{
    public static class PluginLoader
    {

        private static ICollection<IPluginFactory> _factories;
        private static ILogger _logger;

        static PluginLoader()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// creates the component with the given title
        /// </summary>
        /// <typeparam name="T">type of the required component</typeparam>
        /// <param name="title of the required component"></param>
        /// <returns>returns the instantiated component</returns>
        public static T CreateInstance<T>(string title)
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


        /// <summary>
        /// Load the available factories from the assemblies in the given folder
        /// </summary>
        /// <param name="path">path of the folder, where the factories are collected from</param>
        /// <returns>Collection of factories</returns>
        public static bool LoadPlugins(string path)
        {
            if (path == null)
            {
                _logger.Error("Arrived path is null.");
                return false;
            }

            if (!IsPathFolder(path))
            {
                _logger.Error($"Received path is not a directory path: {path}");
            }

            if (Directory.Exists(path))
            {
                // gather all dll names:
                string[] dllFileNames = Directory.GetFiles(path, "*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    try
                    {
                        // load the assembly:
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        Assembly assembly = Assembly.Load(an);
                        assemblies.Add(assembly);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Could not load assembyly from file: {dllFile} -> {ex}");
                    }
                }

                // go through all assmeblies and and check whether they implement IPluginFactory interface

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

                            if (type == typeof(IPluginFactory))
                            {
                                factories.Add((IPluginFactory)Activator.CreateInstance(type));
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


        #region private

        /// <summary>
        /// Checks whether the received path string contains folder path or not
        /// </summary>
        /// <param name="path">the investigated path string</param>
        /// <returns>is it directory path or not</returns>
        private static bool IsPathFolder(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }

            return false;
        }

        #endregion


    }
}
