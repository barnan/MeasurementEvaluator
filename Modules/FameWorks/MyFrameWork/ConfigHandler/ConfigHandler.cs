using MyFrameWork.Interfaces;
using MyFrameWork.PluginLoading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MyFrameWork.ConfigHandler
{
    public class ConfigHandler
    {
        private IMyLogger _logger;
        private string _configFolder;
        private const string _CONFIG_FILE_EXTENSION = ".config";
        private const string _ROOT_NODE_NAME = "Configurations";
        private const string _NAME_ATTRIBUTE_NAME = "Name";
        private const string _VALUE_ATTRIBUTE_NAME = "Value";
        private const string _ASSEMBLY_ATTRIBUTE_NAME = "Assembly";
        private const string _FIELD_NODE_NAME = "Parameter";
        private const string _SECTION_NODE_NAME = "Section";
        private const string _LIST_ELEMENT_NODE_NAME = "Element";


        public ConfigHandler(string folder)
        {
            _logger = PluginLoader.GetLogger(nameof(ConfigHandler));
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

                _logger.Info($"Reading object (type: {type}, namespace {namespaceOfType}) parameters in file: {currentConfigFileName} in section: {sectionName}");

                bool differenceFound = false;

                CreateConfigFileIfNotExisting(currentConfigFileName);

                // load the required section in XElement format:

                XElement searchedSection = LoadSectionXElementFromFile(currentConfigFileName, sectionName, type) ?? CreateSectionXElement(sectionName, type);

                if (!CheckAssemblyAttributeOfSection(searchedSection, type))
                {
                    searchedSection = FixAssembylAttributeOfSection(searchedSection, type);
                    differenceFound = true;
                }

                // edit according to the received parameter object:


                FieldInfo currentObjectField = null;
                FieldInfo[] fieldsWithConfigAttribute = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetCustomAttributes(typeof(ConfigurationAttribute)).Any()).ToArray();
                foreach (var fieldInfo in fieldsWithConfigAttribute)
                {
                    ConfigurationAttribute fieldConfigurationAttribute = (ConfigurationAttribute)fieldInfo.GetCustomAttribute(typeof(ConfigurationAttribute));

                    XAttribute nameAttribute = null;
                    XAttribute valueAttribute = null;
                    IEnumerable<XElement> foundElementSubNodes = null;

                    // get the section with the name of the given property
                    foreach (XNode examinedNode in searchedSection.Nodes())
                    {
                        if (examinedNode is XComment)
                        {
                            continue;
                        }

                        XElement examinedxElement = examinedNode as XElement;

                        nameAttribute = GetAttributeValueByAttributeName(examinedxElement, _NAME_ATTRIBUTE_NAME);
                        valueAttribute = GetAttributeValueByAttributeName(examinedxElement, _VALUE_ATTRIBUTE_NAME);

                        if (nameAttribute == null || valueAttribute == null)
                        {
                            examinedNode.Remove();
                            _logger.Info($"Section ({examinedxElement.Name}) without {_NAME_ATTRIBUTE_NAME} or {_VALUE_ATTRIBUTE_NAME} attribute was found. It is removed.");

                            continue;
                        }

                        if (fieldConfigurationAttribute.Name == nameAttribute.Value)
                        {
                            foundElementSubNodes = examinedxElement.Elements();
                            break;
                        }
                    }

                    // xnode was found for the given field in the section, now get the value:
                    if (nameAttribute != null && valueAttribute != null && fieldConfigurationAttribute.Name == nameAttribute.Value)
                    {
                        try
                        {
                            currentObjectField = fieldInfo;
                            Type fieldType = currentObjectField.FieldType;
                            object temporary;

                            // process list if list was found:
                            if (string.IsNullOrEmpty(valueAttribute.Value) && foundElementSubNodes != null && typeof(IList).IsAssignableFrom(fieldType))
                            {
                                IList listObject = (IList)Activator.CreateInstance(fieldType);

                                foreach (XElement item in foundElementSubNodes)
                                {
                                    valueAttribute = GetAttributeValueByAttributeName(item, _VALUE_ATTRIBUTE_NAME);
                                    string listElement = (string)Convert.ChangeType(valueAttribute.Value, typeof(string));
                                    listObject.Add(listElement);
                                }
                                temporary = listObject;
                            }

                            // process list if dictionary was found:
                            else if (string.IsNullOrEmpty(valueAttribute.Value) && foundElementSubNodes != null && typeof(IDictionary).IsAssignableFrom(fieldType))
                            {
                                IList dictObject = (IList)Activator.CreateInstance(fieldType);

                                foreach (XElement item in foundElementSubNodes)
                                {
                                    valueAttribute = GetAttributeValueByAttributeName(item, _VALUE_ATTRIBUTE_NAME);
                                    string listElement = (string)Convert.ChangeType(valueAttribute.Value, typeof(string));
                                    dictObject.Add(listElement);
                                }
                                temporary = dictObject;
                            }

                            // enum was found
                            else if (fieldType.IsEnum && !string.IsNullOrEmpty(valueAttribute.Value))
                            {
                                temporary = Enum.Parse(fieldType, valueAttribute.Value);
                            }

                            // string was found:
                            else if (fieldType.GenericTypeArguments != null && fieldType.GenericTypeArguments.Length > 0 && fieldType.GenericTypeArguments[0] == typeof(System.String))
                            {
                                // if a string was found -> no conversion is needed:
                                temporary = valueAttribute.Value;
                            }

                            else
                            {
                                // not string, but other singl element -> conversion is needed
                                temporary = Convert.ChangeType(valueAttribute.Value, fieldType);
                            }

                            // set the property value:
                            currentObjectField.SetValue(inputObj, temporary);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Exception occured during {nameAttribute.Value}, {valueAttribute.Value} conversion. {ex.Message}");
                            break;
                        }
                    }
                    else // no section was found in the file for the searched field:
                    {
                        XElement newElement = CreateFieldSectionXElement(fieldConfigurationAttribute.Name, "");
                        XComment comment = CreateXComment(fieldConfigurationAttribute.Description, fieldInfo);

                        searchedSection.Add(comment);
                        searchedSection.Add(newElement);

                        differenceFound = true;
                    }
                }


                // Investigate whether all field sections has pair in the loaded object -> if not make a commented line from it.
                XComment previousComment = null;
                foreach (XNode childXNode in searchedSection.Nodes())
                {
                    XAttribute nameXAttribute = null;

                    if (childXNode is XComment commentNode)
                    {
                        previousComment = commentNode;
                        continue;
                    }

                    XElement childXElement = childXNode as XElement;
                    nameXAttribute = GetAttributeValueByAttributeName(childXElement, _NAME_ATTRIBUTE_NAME);
                    ConfigurationAttribute matchingConfigurationAttribute = null;

                    foreach (var fieldInfo in fieldsWithConfigAttribute)
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
                            previousComment?.Remove();
                            searchedSection.Add(previousComment);
                            searchedSection.Add(comment);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Exception occured during node commenting: {ex.Message}");
                        }

                        differenceFound = true;
                    }
                }

                // save if any change was found during the search:
                if (differenceFound)
                {
                    Save(currentConfigFileName, sectionName, searchedSection, type);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured: {ex}");
                return false;
            }

            return true;
        }

        private bool CheckAssemblyAttributeOfSection(XElement currentSectionElement, Type type)
        {
            string assemblyVersionInfo = currentSectionElement.Attribute(_ASSEMBLY_ATTRIBUTE_NAME)?.Value;

            if (assemblyVersionInfo == null || assemblyVersionInfo != type.Assembly.FullName)
            {
                return false;
            }

            return true;
        }

        private XElement FixAssembylAttributeOfSection(XElement currentSectionElement, Type type)
        {
            string assemblyVersionInfo = currentSectionElement.Attribute(_ASSEMBLY_ATTRIBUTE_NAME).Value;

            if (assemblyVersionInfo != null)
            {
                foreach (XAttribute item in currentSectionElement.Attributes(_ASSEMBLY_ATTRIBUTE_NAME))
                {
                    item.Remove();
                }
            }
            XAttribute attrib = new XAttribute(_ASSEMBLY_ATTRIBUTE_NAME, type.Assembly);
            currentSectionElement.Add(attrib);
            return currentSectionElement;
        }

        internal XElement LoadXmlFromFile(string currentConfigFileName)
        {
            // read in the xml:
            try
            {
                return XElement.Load(currentConfigFileName);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occured loading xml from file ({currentConfigFileName}): {ex.Message}");
                return null;
            }
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

        internal XElement CreateSectionXElement(string sectionName, Type type)
        {
            XElement createdXElement = new XElement(_SECTION_NODE_NAME);

            _logger.Info($"New {sectionName} section was created.");

            XAttribute sectionNameAttribute = new XAttribute(_NAME_ATTRIBUTE_NAME, sectionName);
            XAttribute assemblyAttribute = new XAttribute(_ASSEMBLY_ATTRIBUTE_NAME, type.Assembly);

            createdXElement.Add(sectionNameAttribute);
            createdXElement.Add(assemblyAttribute);

            return createdXElement;
        }


        internal bool Save(string currentConfigFileName, string sectionName, XElement newElement, Type type)
        {
            if (newElement == null)
            {
                _logger.Error("Received element is null, can not be saved.");
                return false;
            }

            // read in the xml:

            XElement rootXml = LoadXmlFromFile(currentConfigFileName);
            XElement oldElement = LoadSectionXElementFromXElement(rootXml, sectionName, type);

            if (rootXml == null || rootXml.NodeType != XmlNodeType.Element)
            {
                rootXml = new XElement(_ROOT_NODE_NAME);
            }

            if (oldElement != null)
            {
                try
                {
                    oldElement.Remove();
                }
                catch (Exception ex)
                {
                    _logger.Error($"Tried to remove XNode: ({oldElement.Name}, {sectionName}), but removing was not successful: {ex.Message}");
                }
            }

            try
            {
                rootXml.Add(newElement);
            }
            catch (Exception ex)
            {
                _logger.Error($"Tried to add child node: ({newElement.Name}, {sectionName}), but adding was not successful: {ex.Message}");
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
                    rootXml.Save(xmlWriter);
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

        private XAttribute GetAttributeValueByAttributeName(XElement element, string attributeName)
        {
            if (element == null)
            {
                return null;
            }

            foreach (XAttribute attribute in element.Attributes())
            {
                if (attribute.Name == attributeName)
                {
                    return attribute;
                }
            }
            return null;
        }


        internal XElement LoadSectionXElementFromFile(string fileName, string sectionName, Type type)
        {
            XElement readXml = LoadXmlFromFile(fileName);
            return LoadSectionXElementFromXElement(readXml, sectionName, type);
        }

        private XElement LoadSectionXElementFromXElement(XElement inputXElement, string sectionName, Type type)
        {
            if (inputXElement == null)
            {
                _logger.Error($"Received ipnutXElement is null, {sectionName} is not found.");
                return null;
            }

            IEnumerable<XElement> childElements = inputXElement.Elements();
            XElement searchedSection = null;

            foreach (XElement element in childElements)
            {
                List<XAttribute> attributeCollection = element.Attributes().ToList();
                if (attributeCollection.Count == 0)
                {
                    _logger.Error($"No attributes was found for node-element: {element.Name.LocalName}");
                }

                foreach (XAttribute attribute in attributeCollection)
                {
                    if (attribute.Name == _NAME_ATTRIBUTE_NAME && attribute.Value == sectionName)
                    {
                        searchedSection = element;
                        break;
                    }
                }

                if (searchedSection != null)
                {
                    break;
                }
            }

            return searchedSection;
        }

        private XComment CreateXComment(string description, FieldInfo fieldInfo = null)
        {
            string enumDescription = string.Empty;
            if (fieldInfo?.FieldType.IsEnum ?? false)
            {
                enumDescription = $"EnumValues: {string.Join(",", Enum.GetNames(fieldInfo.FieldType))}";
            }

            if (string.IsNullOrEmpty(description))
            {
                _logger.Error("Received description is null or empty.");
                return null;
            }
            return new XComment($"{description} {enumDescription}");
        }

        private XElement CreateFieldSectionXElement(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.Error("Received name is null or empty.");
                return null;
            }

            XElement newElement = new XElement(_FIELD_NODE_NAME);

            XAttribute nameAttrib = new XAttribute(_NAME_ATTRIBUTE_NAME, name);
            XAttribute valueAttrib = new XAttribute(_VALUE_ATTRIBUTE_NAME, value);

            newElement.Add(nameAttrib);
            newElement.Add(valueAttrib);

            return newElement;
        }

        #endregion

    }

}
