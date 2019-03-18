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

            _logger.Info($"Reading object (type: {type}) parameters in section name: {sectionName}");
            _logger.Info($"Object (type: {type}) namespace {namespaceOfType}");

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

            // with the config file with the given namespace:

            string[] namespaceFragments = namespaceOfType.Split('.');
            string currentConfigFileName = Path.Combine(_configFolder, namespaceFragments[0] + _configFileExtension);

            if (!Array.Exists<string>(configFiles, p => Path.GetFileNameWithoutExtension(p) == namespaceFragments[0]))
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
                    _logger.Error($"{currentConfigFileName} could not created: {ex.Message}");
                    return false;
                }
            }

            // read in the xml doc

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            using (StreamReader sr = new StreamReader(currentConfigFileName))
            {
                using (XmlReader xmlReader = XmlReader.Create(sr, readerSettings))
                {
                    try
                    {
                        xmlDoc.Load(xmlReader);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Config file ({currentConfigFileName}) could not read: {ex.Message}");
                    }
                }
            }

            if (xmlDoc.DocumentElement == null)
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
                    break;
                }

                foreach (XmlAttribute attribute in attributeCollection)
                {
                    if (attribute.Name == SECTION_NAME_ATTRIBUTE_NAME && attribute.InnerText == sectionName)
                    {
                        currentSectionNode = (XmlElement)node;
                        break;
                    }
                }

                if (currentSectionNode != null)
                {
                    break;
                }
            }

            if (childNodes.Count == 0 || currentSectionNode == null)
            {
                currentSectionNode = xmlDoc.CreateElement(sectionName);

                _logger.Error($"{sectionName} node was not found in {xmlDoc.DocumentElement?.Name} in {currentConfigFileName}. {sectionName} section was created.");

                XmlAttribute sectionNameAttribute = xmlDoc.CreateAttribute(SECTION_NAME_ATTRIBUTE_NAME);
                sectionNameAttribute.InnerText = sectionName;

                XmlAttribute assemblyAttribute = xmlDoc.CreateAttribute("Assembly");
                assemblyAttribute.InnerText = type.Name;

                currentSectionNode.Attributes.Append(sectionNameAttribute);
                currentSectionNode.Attributes.Append(assemblyAttribute);

                xmlDoc.DocumentElement.AppendChild(currentSectionNode);
            }

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

                ConfigurationAttribute atribute = (ConfigurationAttribute)attributes[0];


            }

            // write into the xml file:

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
