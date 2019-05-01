using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Frame.ConfigHandler
{

    // todo: list serialization

    public class ConfigManager
    {
        private ILogger _logger;
        private string _configFolder;
        private const string _CONFIG_FILE_EXTENSION = ".config";
        private const string ROOT_NODE_NAME = "Configurations";
        private const string NAME_ATTRIBUTE_NAME = "Name";
        private const string VALUE_ATTRIBUTE_NAME = "Value";
        private const string ASSEMBLY_ATTRIBUTE_NAME = "Assembly";
        private const string FIELD_NODE_NAME = "Parameter";
        private const string SECTION_NODE_NAME = "Section";
        private const string LIST_ELEMENT_NODE_NAME = "Element";


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

            try
            {
                Type type = inputObj.GetType();
                string namespaceOfType = type.Namespace ?? "Unknown";
                string[] namespaceFragments = namespaceOfType.Split('.');
                string currentConfigFileName = Path.Combine(_configFolder, namespaceFragments[0] + _CONFIG_FILE_EXTENSION);

                _logger.Info($"Reading object (type: {type}) parameters in section name: {sectionName}");
                _logger.Info($"Object (type: {type}) namespace {namespaceOfType}");

                CreateConfigFileIfNotExisting(currentConfigFileName);

                XElement currentSectionElement = LoadSectionXElement(currentConfigFileName, sectionName, type);

                // edit according to the object

                bool differenceFound = false;

                FieldInfo currentObjectField = null;
                FieldInfo[] fieldInfosWithConfigAttribute = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetCustomAttributes(typeof(ConfigurationAttribute)).ToList().Count == 1).ToArray();
                foreach (var fieldInfo in fieldInfosWithConfigAttribute)
                {
                    ConfigurationAttribute fieldConfigurationAttribute = (ConfigurationAttribute)fieldInfo.GetCustomAttribute(typeof(ConfigurationAttribute));

                    XAttribute nameAttribute = null;
                    XAttribute valueAttribute = null;
                    IEnumerable<XElement> xmlListElementNode = null;

                    foreach (XElement parameterNode in currentSectionElement.Nodes())
                    {
                        if (parameterNode is XComment)       // todo
                        {
                            continue;
                        }

                        if (!(parameterNode is XElement parameterElement))
                        {
                            continue;       // todo
                        }

                        xmlListElementNode = parameterNode.Elements();
                        nameAttribute = GetAttributeValueByAttributeName(parameterElement, NAME_ATTRIBUTE_NAME);
                        valueAttribute = GetAttributeValueByAttributeName(parameterElement, VALUE_ATTRIBUTE_NAME);

                        if (nameAttribute == null || valueAttribute == null)
                        {
                            parameterNode.Remove();
                            _logger.Info($"XNode ({parameterNode.Name}) without {NAME_ATTRIBUTE_NAME} or {VALUE_ATTRIBUTE_NAME} attribute was found. It is removed.");

                            continue;
                        }

                        if (fieldConfigurationAttribute.Name == nameAttribute.Value)
                        {
                            break;
                        }
                    }

                    // xmlnode was found for the given field in the xml section:
                    if (nameAttribute != null && valueAttribute != null && fieldConfigurationAttribute.Name == nameAttribute.Value)
                    {
                        try
                        {
                            currentObjectField = fieldInfo;
                            Type fieldType = currentObjectField.FieldType;

                            // process list if list was found:
                            if (valueAttribute.Value == string.Empty && xmlListElementNode != null && typeof(ICollection).IsAssignableFrom(fieldType))
                            {
                                IList listobj = (IList)Activator.CreateInstance(fieldType);

                                foreach (XElement item in xmlListElementNode)
                                {
                                    valueAttribute = GetAttributeValueByAttributeName(item, VALUE_ATTRIBUTE_NAME);
                                    string listElement = (string)Convert.ChangeType(valueAttribute.Value, typeof(string));
                                    listobj.Add(listElement);
                                }
                                currentObjectField.SetValue(inputObj, listobj);
                            }
                            else    // not list, but a single lement was found
                            {
                                object temporary;
                                // if a string was found -> no conversion is needed:
                                if (fieldType.GenericTypeArguments != null && fieldType.GenericTypeArguments.Length > 0 && fieldType.GenericTypeArguments[0] == typeof(System.String))
                                {
                                    temporary = valueAttribute.Value;
                                }
                                else    // not string -> conversion is needed
                                {
                                    temporary = Convert.ChangeType(valueAttribute.Value, fieldType);
                                }

                                currentObjectField.SetValue(inputObj, temporary);
                            }

                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Exception occured during {nameAttribute.Value}, {valueAttribute.Value} conversion: {ex.Message}");
                            break;
                        }
                    }
                    else // no xml section was found in the file for the searched field:
                    {
                        XElement newElement = CreateFieldSectionXElement(fieldConfigurationAttribute.Name, "");
                        XComment comment = CreateXComment(fieldConfigurationAttribute.Description);

                        currentSectionElement.Add(comment);
                        currentSectionElement.Add(newElement);

                        differenceFound = true;
                    }
                }


                // Investigate whether all field sections has pair in the loaded object
                XComment previousComment = null;
                foreach (XNode childXNode in currentSectionElement.Nodes())
                {
                    XAttribute nameXAttribute = null;

                    if (childXNode is XComment || childXNode == null)
                    {
                        previousComment = (XComment)childXNode;
                        continue;
                    }

                    XElement childXElement = (XElement)childXNode;
                    nameXAttribute = GetAttributeValueByAttributeName(childXElement, NAME_ATTRIBUTE_NAME);
                    ConfigurationAttribute matchingConfigurationAttribute = null;

                    foreach (var fieldInfo in fieldInfosWithConfigAttribute)
                    {
                        ConfigurationAttribute fieldConfigurationAttribute = (ConfigurationAttribute)fieldInfo.GetCustomAttribute(typeof(ConfigurationAttribute));

                        if (fieldConfigurationAttribute.Name == nameXAttribute.Value)
                        {
                            matchingConfigurationAttribute = fieldConfigurationAttribute;
                            break;
                        }
                    }

                    if (matchingConfigurationAttribute == null)
                    {
                        XComment comment = CreateXComment(childXNode.ToString());
                        try
                        {
                            childXNode.Remove();
                            previousComment.Remove();
                            currentSectionElement.Add(previousComment);
                            currentSectionElement.Add(comment);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Exception occured during node commenting: {ex.Message}");
                        }

                        differenceFound = true;
                    }
                }

                if (differenceFound)
                {
                    Save(currentConfigFileName, sectionName, currentSectionElement, type);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
                return false;
            }

            return true;
        }


        internal void CreateConfigFileIfNotExisting(string currentConfigFileName)
        {
            //get config file list:
            string[] configFiles = Directory.GetFiles(_configFolder, "*" + _CONFIG_FILE_EXTENSION, SearchOption.TopDirectoryOnly);

            if (!Array.Exists<string>(configFiles, p => Path.GetFileName(p) == Path.GetFileName(currentConfigFileName)))
            {
                _logger.Error($"{currentConfigFileName} file was NOT found in {_configFolder}");

                FileStream fs = File.Create(currentConfigFileName);
                fs.Close();

                _logger.Info($"{currentConfigFileName} created.");
            }
        }


        internal XmlElement LoadXmlElementFromFile(XmlDocument xmlDoc, string currentConfigFileName, string sectionName)
        {
            // load xmleelement from file:

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
                        _logger.Error($"Config file ({currentConfigFileName}) could NOT read: {ex.Message}");
                        return null;
                    }
                }
            }

            if (xmlDoc.DocumentElement == null)
            {
                return null;
            }

            // load the reuired section from the XElement:

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
                    if (attribute.Name == NAME_ATTRIBUTE_NAME && attribute.InnerText == sectionName)
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



        internal XElement CreateSectionXElement(string sectionName, Type type)
        {
            XElement createdXElement = new XElement(SECTION_NODE_NAME);

            _logger.Info($"New {sectionName} section was created.");

            XAttribute sectionNameAttribute = new XAttribute(NAME_ATTRIBUTE_NAME, sectionName);
            XAttribute assemblyAttribute = new XAttribute(ASSEMBLY_ATTRIBUTE_NAME, type.Name);

            createdXElement.Add(sectionNameAttribute);
            createdXElement.Add(assemblyAttribute);

            return createdXElement;
        }


        internal XmlElement CreateXmlSection(XmlDocument xmlDoc, string sectionName, Type type)
        {
            XmlElement createdSectionNode = xmlDoc.CreateElement(SECTION_NODE_NAME);

            _logger.Info($"New {sectionName} section was created.");

            XmlAttribute sectionNameAttribute = xmlDoc.CreateAttribute(NAME_ATTRIBUTE_NAME);
            sectionNameAttribute.InnerText = sectionName;

            // creates additional attributes for the new node -> assembly name
            XmlAttribute assemblyAttribute = xmlDoc.CreateAttribute(ASSEMBLY_ATTRIBUTE_NAME);
            assemblyAttribute.InnerText = type.Name;

            createdSectionNode.Attributes.Append(sectionNameAttribute);
            createdSectionNode.Attributes.Append(assemblyAttribute);

            return createdSectionNode;
        }


        internal bool Save(string currentConfigFileName, string sectionName, XElement newElement, Type type)
        {
            if (newElement == null)
            {
                _logger.Error("Received element is null, can not be saved.");
                return false;
            }

            //// read in the xml doc

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement oldElement = LoadXmlElementFromFile(xmlDoc, currentConfigFileName, sectionName);

            if (xmlDoc.DocumentElement == null)
            {
                XmlNode rootNode = xmlDoc.CreateElement(ROOT_NODE_NAME);
                xmlDoc.AppendChild(rootNode);
            }

            if (oldElement != null)
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
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, writerSettings))
                {
                    xmlDoc.WriteTo(xmlWriter);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Config file ({currentConfigFileName}) could not write: {ex.Message}");
                return false;
            }

            return true;
        }

        #region private

        private XmlAttribute GetAttributeValueByAttributeName(XmlNode node, string attributeName)
        {
            foreach (XmlAttribute childNodeAttribute in node.Attributes)
            {
                if (childNodeAttribute.Name == attributeName)
                {
                    return childNodeAttribute;
                }
                if (childNodeAttribute.Name == attributeName)
                {
                    return childNodeAttribute;
                }
            }

            return null;
        }

        private XAttribute GetAttributeValueByAttributeName(XElement element, string attributeName)
        {
            foreach (XAttribute attribute in element.Attributes())
            {
                if (attribute.Name == attributeName)
                {
                    return attribute;
                }
            }
            return null;
        }


        private XmlElement LoadXmlElement(string currentConfigFileName, string sectionName, Type type)
        {
            // read in the xml doc

            XmlDocument xmlDoc = new XmlDocument();

            XmlElement currentSectionNode = LoadXmlElementFromFile(xmlDoc, currentConfigFileName, sectionName);

            if (currentSectionNode == null)
            {
                _logger.Info("Null node received.");

                currentSectionNode = CreateXmlSection(xmlDoc, sectionName, type);
            }

            return currentSectionNode;
        }

        internal XElement LoadSectionXElement(string currentConfigFileName, string sectionName, Type type)
        {
            // read in the xml 

            XElement readElement = XElement.Load(currentConfigFileName);

            IEnumerable<XElement> childElements = readElement.Elements();
            XElement currentSection = null;

            foreach (XElement element in childElements)
            {
                IEnumerable<XAttribute> attributeCollection = element.Attributes();
                if (attributeCollection == null)
                {
                    _logger.Error($"No attributes was found for node-element: {element.Name.LocalName}");
                    continue;
                }

                foreach (XAttribute attribute in attributeCollection)
                {
                    if (attribute.Name == NAME_ATTRIBUTE_NAME && attribute.Value == sectionName)
                    {
                        currentSection = element;
                        break;
                    }
                }

                if (currentSection != null)
                {
                    break;
                }
            }

            if (currentSection == null)
            {
                _logger.Info($"Section ({sectionName}) was not found.");

                currentSection = CreateSectionXElement(sectionName, type);
            }

            return currentSection;
        }

        private XmlComment CreateFieldXmlComment(XmlDocument xmlDoc, string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                _logger.Error("Received description is null or empty.");
                return null;
            }
            XmlComment newComment = xmlDoc.CreateComment(description);
            return newComment;
        }

        private XComment CreateXComment(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                _logger.Error("Received description is null or empty.");
                return null;
            }
            return new XComment(description);
        }

        private XElement CreateFieldSectionXElement(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.Error("Received name is null or empty.");
                return null;
            }

            XElement newElement = new XElement(FIELD_NODE_NAME);

            XAttribute nameAttrib = new XAttribute(NAME_ATTRIBUTE_NAME, name);
            XAttribute valueAttrib = new XAttribute(VALUE_ATTRIBUTE_NAME, value);

            newElement.Add(nameAttrib);
            newElement.Add(valueAttrib);

            return newElement;
        }


        private XmlElement CreateFieldXmlSection(XmlDocument xmlDoc, string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.Error("Received name is null or empty.");
                return null;
            }

            XmlElement newElement = xmlDoc.CreateElement(FIELD_NODE_NAME);

            XmlAttribute nameAttrib = xmlDoc.CreateAttribute(NAME_ATTRIBUTE_NAME);
            nameAttrib.Value = name;
            XmlAttribute valueAttrib = xmlDoc.CreateAttribute(VALUE_ATTRIBUTE_NAME);
            valueAttrib.Value = value;

            newElement.Attributes.Append(nameAttrib);
            newElement.Attributes.Append(valueAttrib);

            return newElement;
        }

        #endregion

    }
}
