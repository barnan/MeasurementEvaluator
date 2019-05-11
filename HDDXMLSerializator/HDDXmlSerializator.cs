using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.Misc;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DataAcquisitions.HDDXmlSerializator
{
    internal class HDDXmlSerializator : IHDDFileReaderWriter
    {

        private readonly HDDXmlSerializatorParameters _parameters;
        private readonly object _lockObject = new object();


        internal HDDXmlSerializator(HDDXmlSerializatorParameters parameter)
        {
            _parameters = parameter;
        }


        public object ReadFromFile(string fileNameAndPath, ToolNames toolName = null)
        {
            lock (_lockObject)
            {
                try
                {
                    if (!CanRead(fileNameAndPath))
                    {
                        _parameters.Logger.Error($"File is not accessible: {fileNameAndPath}");
                    }

                    XElement readElement = XElement.Load(fileNameAndPath);

                    XAttribute assembylAttribute = readElement.Attribute("Assembly");

                    string readTypeName = assembylAttribute?.Value ?? readElement.Name.LocalName;

                    Type readType = Type.GetType(readTypeName);
                    object createdObj = null;

                    if (typeof(IXmlStorable).IsAssignableFrom(readType))
                    {
                        createdObj = Activator.CreateInstance(readType);
                        (createdObj as IXmlStorable).LoadFromXml(readElement);
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(fileNameAndPath))
                        {
                            XmlSerializer serializer = new XmlSerializer(readType);
                            createdObj = serializer.Deserialize(sr);
                            return true;
                        }

                    }

                    return createdObj;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.Error($"Exception occured: {ex}");
                    return false;
                }
            }
        }


        public bool WriteToFile(object tobj, string fileNameAndPath)
        {
            lock (_lockObject)
            {
                try
                {
                    Type type = tobj.GetType();

                    if (tobj is IXmlStorable storable)
                    {
                        XElement element = new XElement(type.Name);
                        XAttribute assemblyAttrib = new XAttribute("Assembly", type.AssemblyQualifiedName);
                        element.Add(assemblyAttrib);

                        storable.SaveToXml(element);

                        element.Save(fileNameAndPath);
                        return true;
                    }

                    using (StreamWriter sw = new StreamWriter(fileNameAndPath))
                    {
                        XmlSerializer serializer = new XmlSerializer(type);
                        serializer.Serialize(sw, tobj);
                        return true;
                    }

                    _parameters.Logger.Error($"Received object is not {nameof(IXmlStorable)}");


                    return false;
                }
                catch (Exception ex)
                {
                    _parameters.Logger.Error($"Exception occured: {ex}");
                    return false;
                }
            }
        }


        public bool CanRead(string fileNameAndPath)
        {
            if (!string.IsNullOrEmpty(fileNameAndPath))
            {
                using (FileStream fstream = new FileStream(fileNameAndPath, FileMode.Open, FileAccess.Read))
                {
                    if (fstream.CanRead)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private bool CheckFilePath(string fileNameAndPath)
        {
            return File.Exists(fileNameAndPath);
        }

    }
}
