using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FrameInterfaces;

namespace Frame.Configuration
{
    public class ConfigManager
    {
        private IMyLogger _logger;
        private string _configFolder;
        private const string CONFIG_FILE_EXTENSION = ".config";
        private const string ROOT_NODE_NAME = "Configurations";
        private const string NAME_ATTRIBUTE_NAME = "Name";
        private const string VALUE_ATTRIBUTE_NAME = "Value";
        private const string ASSEMBLY_ATTRIBUTE_NAME = "Assembly";
        private const string FIELD_NODE_NAME = "Parameter";
        private const string SECTION_NODE_NAME = "Section";
        private const string LIST_ELEMENT_NODE_NAME = "Element";


        public ConfigManager(string folder)
        {
            _logger = PluginLoader.PluginLoader.GetLogger(nameof(ConfigManager));
            _configFolder = folder;
        }


        public bool Load(object inputObj, string sectionName)
        {
            if (inputObj == null)
            {
                _logger.Error("Received object is null");
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
                string currentConfigFileName = Path.Combine(_configFolder, namespaceFragments[0] + CONFIG_FILE_EXTENSION);

                _logger.Info($"Reading object (type: {type}) parameters in section name: {sectionName}");
                _logger.Info($"Object (type: {type}) namespace {namespaceOfType}");

                bool differenceFound = false;

                CreateConfigFileIfNotExisting(currentConfigFileName);

                // load the required section in XElement format:

                XElement currentSectionElement = LoadSectionFromFile(currentConfigFileName, sectionName);

                if (currentSectionElement == null)
                {
                    currentSectionElement = CreateSection(sectionName, type.Assembly.ToString());
                }

                if (!CheckAssemblyAttributeOfSection(currentSectionElement, type))
                {
                    currentSectionElement = FixAssemblyAttributeOfSection(currentSectionElement, type);
                    differenceFound = true;
                }

                // edit according to the received parameter object:


                FieldInfo currentObjectField = null;
                FieldInfo[] fieldInfosWithConfigAttribute = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetCustomAttributes(typeof(ConfigurationAttribute)).ToList().Count == 1).ToArray();
                foreach (var fieldInfo in fieldInfosWithConfigAttribute)
                {
                    ConfigurationAttribute fieldConfigurationAttribute = (ConfigurationAttribute)fieldInfo.GetCustomAttribute(typeof(ConfigurationAttribute));

                    XAttribute nameAttribute = null;
                    XAttribute valueAttribute = null;
                    IEnumerable<XElement> xmlListElementNode = null;

                    foreach (XNode parameterNode in currentSectionElement.Nodes())
                    {
                        if (parameterNode is XComment)       // todo
                        {
                            continue;
                        }

                        if (!(parameterNode is XElement parameterElement))
                        {
                            continue;       // todo
                        }

                        xmlListElementNode = parameterElement.Elements();
                        nameAttribute = GetAttributeValueByAttributeName(parameterElement, NAME_ATTRIBUTE_NAME);
                        valueAttribute = GetAttributeValueByAttributeName(parameterElement, VALUE_ATTRIBUTE_NAME);

                        if (nameAttribute == null || valueAttribute == null)
                        {
                            parameterNode.Remove();
                            _logger.Info($"Section ({parameterElement.Name}) without {NAME_ATTRIBUTE_NAME} or {VALUE_ATTRIBUTE_NAME} attribute was found. It is removed");

                            continue;
                        }

                        if (fieldConfigurationAttribute.Name == nameAttribute.Value)
                        {
                            break;
                        }
                    }

                    // xnode was found for the given field in the section:
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
                            else // not list, but a single lement was found
                            {
                                object temporary;
                                // if a string was found -> no conversion is needed:
                                if (fieldType.IsEnum && !string.IsNullOrEmpty(valueAttribute.Value))
                                {
                                    temporary = Enum.Parse(fieldType, valueAttribute.Value);
                                }
                                else
                                {
                                    if (fieldType.GenericTypeArguments != null && fieldType.GenericTypeArguments.Length > 0 && fieldType.GenericTypeArguments[0] == typeof(System.String))
                                    {
                                        temporary = valueAttribute.Value;
                                    }
                                    else // not string -> conversion is needed
                                    {
                                        temporary = Convert.ChangeType(valueAttribute.Value, fieldType);
                                    }
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
                    else // no section was found in the file for the searched field:
                    {
                        XElement newElement = CreateFieldSectionXElement(fieldConfigurationAttribute.Name, "");
                        XComment comment = CreateXComment(fieldConfigurationAttribute.Description, fieldInfo);

                        currentSectionElement.Add(comment);
                        currentSectionElement.Add(newElement);

                        differenceFound = true;
                    }
                }


                // Investigate whether all field sections has pair in the loaded object -> if not make comment from it.
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
                    Save(currentConfigFileName, sectionName, currentSectionElement);
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
            string assemblyVersionInfo = currentSectionElement.Attribute(ASSEMBLY_ATTRIBUTE_NAME).Value;

            if (assemblyVersionInfo == null || assemblyVersionInfo != type.Assembly.FullName)
            {
                return false;
            }

            return true;    // todo befejezni
        }

        private XElement FixAssemblyAttributeOfSection(XElement currentSectionElement, Type type)
        {
            string assemblyVersionInfo = currentSectionElement.Attribute(ASSEMBLY_ATTRIBUTE_NAME).Value;

            if (assemblyVersionInfo != null)
            {
                foreach (XAttribute item in currentSectionElement.Attributes(ASSEMBLY_ATTRIBUTE_NAME))
                {
                    item.Remove();
                }
            }
            XAttribute attribute = new XAttribute(ASSEMBLY_ATTRIBUTE_NAME, type.Assembly);
            currentSectionElement.Add(attribute);
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
                _logger.Error($"Exception occurred loading xml from file ({currentConfigFileName}): {ex.Message}");
                return null;
            }
        }

        internal void CreateConfigFileIfNotExisting(string currentConfigFileName)
        {
            //get config file list:
            string[] configFiles = Directory.GetFiles(_configFolder, "*" + CONFIG_FILE_EXTENSION, SearchOption.TopDirectoryOnly);

            if (Array.Exists<string>(configFiles, p => Path.GetFileName(p) == Path.GetFileName(currentConfigFileName)))
            {
                return;
            }
            
            _logger.Info($"{currentConfigFileName} file was NOT found in {_configFolder}");

            using (FileStream fs = File.Create(currentConfigFileName))
            {
                fs.Close();
            }

            _logger.Info($"{currentConfigFileName} created");
        }

        internal XElement CreateSection(string sectionName, string typeString)
        {
            XElement createdXElement = new XElement(SECTION_NODE_NAME);

            _logger.Info($"New {sectionName} section was created");

            XAttribute sectionNameAttribute = new XAttribute(NAME_ATTRIBUTE_NAME, sectionName);
            XAttribute assemblyAttribute = new XAttribute(ASSEMBLY_ATTRIBUTE_NAME, typeString);

            createdXElement.Add(sectionNameAttribute);
            createdXElement.Add(assemblyAttribute);

            return createdXElement;
        }


        internal bool Save(string currentConfigFileName, string sectionName, XElement newElement)
        {
            if (newElement == null)
            {
                _logger.Error("Received element is null, can not be saved");
                return false;
            }

            // read in the xml:

            XElement rootXml = LoadXmlFromFile(currentConfigFileName);
            XElement oldElement = LoadSectionXElementFromXElement(rootXml, sectionName);

            if (rootXml == null || rootXml.NodeType != XmlNodeType.Element)
            {
                rootXml = new XElement(ROOT_NODE_NAME);
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

        internal XAttribute GetAttributeValueByAttributeName(XElement element, string attributeName)
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


        internal XElement LoadSectionFromFile(string fileName, string sectionName)
        {
            XElement readXml = LoadXmlFromFile(fileName);

            XElement currentSectionElement = LoadSectionXElementFromXElement(readXml, sectionName);

            return currentSectionElement;
        }

        internal XElement LoadSectionXElementFromXElement(XElement inputXElement, string sectionName)
        {
            if (inputXElement == null)
            {
                _logger.Error($"Received inputXElement is null, {sectionName} is not found");
                return null;
            }

            IEnumerable<XElement> childElements = inputXElement.Elements();

            foreach (XElement element in childElements)
            {
                IEnumerable<XAttribute> attributeCollection = element.Attributes();
                if (!attributeCollection.Any())
                {
                    _logger.Error($"No attributes was found for node-element: {element.Name.LocalName}");
                    continue;
                }

                if (attributeCollection.Any(attribute => attribute.Name == NAME_ATTRIBUTE_NAME && attribute.Value == sectionName))
                {
                    return element;
                }
            }

            return null;
        }

        internal XComment CreateXComment(string description, FieldInfo fieldInfo = null)
        {
            string enumDescription = string.Empty;
            if (fieldInfo?.FieldType.IsEnum ?? false)
            {
                enumDescription = $"EnumValues: {string.Join(",", Enum.GetNames(fieldInfo.FieldType))}";
            }

            if (string.IsNullOrEmpty(description))
            {
                _logger.Error("Received description is null or empty");
                return null;
            }
            return new XComment($"{description} {enumDescription}");
        }

        internal XElement CreateFieldSectionXElement(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.Error("Received name is null or empty");
                return null;
            }

            XElement newElement = new XElement(FIELD_NODE_NAME);

            XAttribute nameAttrib = new XAttribute(NAME_ATTRIBUTE_NAME, name);
            XAttribute valueAttrib = new XAttribute(VALUE_ATTRIBUTE_NAME, value);

            newElement.Add(nameAttrib);
            newElement.Add(valueAttrib);

            return newElement;
        }

        #endregion

    }
}
