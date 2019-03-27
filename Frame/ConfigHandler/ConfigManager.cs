using NLog;
using System;
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
        private const string SECTION_NAME_ATTRIBUTE_NAME = "Name";
        private const string Assembly_NAME_ATTRIBUTE_NAME = "Assembly";
        private const string SECTION_NAME = "Section";
        private const string VALUE_NAME_ATTRIBUTE_NAME = "Value";


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

            if (!CheckOrCreateConfigFile(sectionName, currentConfigFileName))
            {
                return false;
            }

            XmlElement currentSectionElement = LoadXmlElement(currentConfigFileName, sectionName, type);

            // edit according to the object

            bool differenceFound = false;

            FieldInfo currentObjectField = null;
            FieldInfo[] fieldInfosWithConfigAttribute = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetCustomAttributes(typeof(ConfigurationAttribute)).ToList().Count == 1).ToArray();
            foreach (var fieldInfo in fieldInfosWithConfigAttribute)
            {
                ConfigurationAttribute fieldConfigurationAttribute = (ConfigurationAttribute)fieldInfo.GetCustomAttribute(typeof(ConfigurationAttribute));

                XmlAttribute xmlNameAttribute = null;
                XmlAttribute xmlValueAttribute = null;

                foreach (XmlNode xmlChildNode in currentSectionElement.ChildNodes)
                {
                    if (xmlChildNode is XmlComment)
                    {
                        continue;
                    }

                    foreach (XmlAttribute childNodeAttribute in xmlChildNode.Attributes)
                    {
                        if (childNodeAttribute.Name == SECTION_NAME_ATTRIBUTE_NAME)
                        {
                            xmlNameAttribute = childNodeAttribute;
                        }
                        if (childNodeAttribute.Name == VALUE_NAME_ATTRIBUTE_NAME)
                        {
                            xmlValueAttribute = childNodeAttribute;
                        }
                    }

                    if (xmlNameAttribute == null || xmlValueAttribute == null)
                    {
                        _logger.Error($"XmlNode ({xmlChildNode.Name}) without {SECTION_NAME_ATTRIBUTE_NAME} or {VALUE_NAME_ATTRIBUTE_NAME} attribute was found. It is removed.");
                        try
                        {
                            currentSectionElement.RemoveChild(xmlChildNode);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Section ({xmlChildNode.InnerText}) tried to remove, but exception occured: {ex.Message}");
                        }
                        continue;
                    }

                    if (fieldConfigurationAttribute.Name == xmlNameAttribute.InnerText)
                    {
                        break;
                    }
                }

                // no xmlnode was found for the given field in the xml section:
                if (xmlNameAttribute != null && xmlValueAttribute != null)
                {
                    currentObjectField = fieldInfo;
                    Type fieldType = currentObjectField.FieldType;

                    object temporary = null;
                    try
                    {
                        temporary = Convert.ChangeType(xmlValueAttribute.InnerText, fieldType);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Exception occured during {xmlNameAttribute.InnerText}, {xmlValueAttribute.InnerText} conversion: {ex.Message}");
                        break;
                    }

                    currentObjectField.SetValue(inputObj, temporary);
                }

                // create a new section for the field:
            }

            return Save(currentConfigFileName, sectionName, currentSectionElement, type);
        }


        internal bool CheckOrCreateConfigFile(string sectionName, string currentConfigFileName)
        {
            //get config file list:
            string[] configFiles;
            try
            {
                configFiles = Directory.GetFiles(_configFolder, "*" + _configFileExtension, SearchOption.TopDirectoryOnly);
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


        internal XmlElement LoadXmlElement(string currentConfigFileName, string sectionName, Type type)
        {
            // read in the xml doc

            XmlDocument xmlDoc = new XmlDocument();

            XmlElement currentSectionNode = LoadXmlElement(xmlDoc, currentConfigFileName, sectionName);

            if (currentSectionNode == null)
            {
                _logger.Info("Null node received.");

                currentSectionNode = CreateXmlSection(xmlDoc, sectionName, type);
            }

            return currentSectionNode;
        }


        internal XmlElement LoadXmlElement(XmlDocument xmlDoc, string currentConfigFileName, string sectionName)
        {
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
                        return null;
                    }
                }
            }

            if (xmlDoc.DocumentElement == null)
            {
                return null;
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

                if (currentSectionNode != null)
                {
                    break;
                }
            }

            return currentSectionNode;
        }


        internal XmlElement CreateXmlSection(XmlDocument xmlDoc, string sectionName, Type type)
        {
            XmlElement createdSectionNode = xmlDoc.CreateElement(SECTION_NAME);

            _logger.Info($"New {sectionName} section was created.");

            XmlAttribute sectionNameAttribute = xmlDoc.CreateAttribute(SECTION_NAME_ATTRIBUTE_NAME);
            sectionNameAttribute.InnerText = sectionName;

            // creates additional attributes for the new node -> assembly name
            XmlAttribute assemblyAttribute = xmlDoc.CreateAttribute(Assembly_NAME_ATTRIBUTE_NAME);
            assemblyAttribute.InnerText = type.Name;

            createdSectionNode.Attributes.Append(sectionNameAttribute);
            createdSectionNode.Attributes.Append(assemblyAttribute);

            return createdSectionNode;
        }


        internal bool Save(string currentConfigFileName, string sectionName, XmlElement newElement, Type type)
        {
            if (newElement == null)
            {
                _logger.Error("Received element is null.");
                return false;
            }

            //// read in the xml doc

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement oldElement = LoadXmlElement(xmlDoc, currentConfigFileName, sectionName);

            if (xmlDoc.DocumentElement == null)
            {
                XmlNode rootNode = xmlDoc.CreateElement(ROOT_NODE_NAME);
                xmlDoc.AppendChild(rootNode);
            }

            if (oldElement == null)
            {
                _logger.Info("Null node received.");

                oldElement = CreateXmlSection(xmlDoc, sectionName, type);
            }
            else
            {
                try
                {
                    xmlDoc.DocumentElement.RemoveChild(oldElement);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Tried to remove node: ({oldElement.Name}, {sectionName}), but removing was not successful: {ex.Message}");
                }
            }

            try
            {
                XmlNode importNode = xmlDoc.ImportNode(newElement, true);
                xmlDoc.DocumentElement.AppendChild(importNode);
            }
            catch (Exception ex)
            {
                _logger.Error($"Tried to append child node: ({newElement.Name}, {sectionName}), but adding was not successful: {ex.Message}");
                return false;
            }

            //write into the xml file:

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = Encoding.UTF8;
            writerSettings.Indent = true;
            writerSettings.CloseOutput = true;
            try
            {
                using (StreamWriter sw = new StreamWriter(currentConfigFileName))
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(sw, writerSettings))
                    {
                        xmlDoc.WriteTo(xmlWriter);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Config file ({currentConfigFileName}) could not write: {ex.Message}");
                return false;
            }

            return true;
        }


    }
}
