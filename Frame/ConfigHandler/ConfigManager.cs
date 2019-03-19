using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Frame.ConfigHandler
{
    public class ConfigManager
    {
        private ILogger _logger;
        private string _configFolder;
        private readonly string _configFileExtension = ".config";
        private const string ROOT_NODE_NAME = "Configurations";
        private const string SECTION_NAME_ATTRIBUTE_NAME = "SectionName";

        public ConfigManager(string folder)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _configFolder = folder;
        }



        public bool Load(object inputObj, string sectionName)
        {
            if (inputObj == null)
            {
                _logger.Error("Received object is null.");
                return false;
            }

            if (string.IsNullOrEmpty(sectionName) || string.IsNullOrWhiteSpace(sectionName))
            {
                _logger.Error($"Received {nameof(sectionName)} is null, empty, or white space");
                return false;
            }

            Type type = inputObj.GetType();
            string namespaceOfType = type.Namespace ?? "Unknown";
            string[] namespaceFragments = namespaceOfType.Split('.');
            string currentConfigFileName = Path.Combine(_configFolder, namespaceFragments[0] + _configFileExtension);

            _logger.Info($"Reading object (type: {type}) parameters in section name: {sectionName}");
            _logger.Info($"Object (type: {type}) namespace {namespaceOfType}");

            if (CheckOrCreateConfigFile(sectionName, currentConfigFileName))
            {
                return false;
            }

            XmlElement currentSectionElement = Load(type, sectionName, currentConfigFileName);


            // edit according to the object

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                List<Attribute> attributes = fieldInfo.GetCustomAttributes(typeof(ConfigurationAttribute)).ToList();

                if (attributes.Count == 0)
                {
                    continue;
                }

                if (attributes.Count > 1)
                {
                    _logger.Error($"More configuration attributes are attached to {type.Name}");
                    break;
                }

                ConfigurationAttribute attribute = (ConfigurationAttribute)attributes[0];

                if (attribute.LoadComponent)
                {
                    //var component = PluginLoader.PluginLoader.CreateInstance<>()
                }


            }

            return true;
        }



        internal bool CheckOrCreateConfigFile(string sectionName, string currentConfigFileName)
        {
            //get config file list:
            string[] configFiles;
            try
            {
                configFiles = Directory.GetFiles(_configFolder, "*" + _configFileExtension, SearchOption.TopDirectoryOnly);
                if (configFiles.Length == 0)
                {
                    _logger.Error($"No config files were found in folder: {_configFolder}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured during config file search for {sectionName} in folder: {_configFolder}{Environment.NewLine}{ex.Message}");
                return false;
            }

            if (!Array.Exists<string>(configFiles, p => Path.GetFileName(p) == Path.GetFileName(currentConfigFileName)))
            {
                _logger.Error($"{currentConfigFileName} file was not found.");

                try
                {
                    FileStream fs = File.Create(currentConfigFileName);
                    fs.Close();
                    _logger.Info($"{currentConfigFileName} created.");
                }
                catch (Exception ex)
                {
                    _logger.Error($"{currentConfigFileName} was NOT existing and could NOT created: {ex.Message}");
                    return false;
                }
            }
            return true;
        }


        internal XmlElement Load(Type type, string sectionName, string currentConfigFileName)
        {
            // read in the xml doc

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            using (StreamReader sr = new StreamReader(currentConfigFileName))
            {
                using (XmlReader xmlReader = XmlReader.Create(sr, readerSettings))
                {
                    try
                    {
                        xmlDoc.Load(xmlReader);     // throws an exception if the config file was created in the previous code-section above
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Config file ({currentConfigFileName}) could not read: {ex.Message}");
                    }
                }
            }

            if (xmlDoc.DocumentElement == null)     // if the xml was "empty"
            {
                XmlNode rootNode = xmlDoc.CreateElement(ROOT_NODE_NAME);
                xmlDoc.AppendChild(rootNode);
            }

            XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
            XmlElement currentSectionNode = null;

            foreach (XmlNode node in childNodes)
            {
                XmlAttributeCollection attributeCollection = node.Attributes;
                if (attributeCollection == null)
                {
                    _logger.Error($"No attributes was found for node-element: {node.Name}");
                    continue;
                }

                foreach (XmlAttribute attribute in attributeCollection)
                {
                    if (attribute.Name == SECTION_NAME_ATTRIBUTE_NAME && attribute.InnerText == sectionName)
                    {
                        currentSectionNode = (XmlElement)node;
                        break;
                    }
                }

                if (currentSectionNode != null)     // if the inner loop has found the searched attribute for the node
                {
                    break;
                }
            }

            if (childNodes.Count == 0 || currentSectionNode == null)     // if the node was not found, or its is empty
            {
                currentSectionNode = xmlDoc.CreateElement(sectionName);

                _logger.Error($"{sectionName} node was not found in {xmlDoc.DocumentElement?.Name} in {currentConfigFileName}.New {sectionName} section was created.");

                XmlAttribute sectionNameAttribute = xmlDoc.CreateAttribute(SECTION_NAME_ATTRIBUTE_NAME);
                sectionNameAttribute.InnerText = sectionName;

                // creates additional attributes for the new node -> assembly name
                XmlAttribute assemblyAttribute = xmlDoc.CreateAttribute("Assembly");
                assemblyAttribute.InnerText = type.Name;

                currentSectionNode.Attributes.Append(sectionNameAttribute);
                currentSectionNode.Attributes.Append(assemblyAttribute);

                //xmlDoc.DocumentElement.AppendChild(currentSectionNode);
            }

            return currentSectionNode;
        }

        internal bool Save(XmlElement newElement, string sectionName, string currentConfigFileName)
        {
            //write into the xml file:

            XmlDocument xmlDoc = new XmlDocument();

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = Encoding.UTF8;
            writerSettings.Indent = true;
            writerSettings.CloseOutput = true;

            using (StreamWriter sw = new StreamWriter(currentConfigFileName))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, writerSettings))
                {
                    xmlDoc.WriteTo(xmlWriter);
                }
            }

            return true;
        }


    }
}
