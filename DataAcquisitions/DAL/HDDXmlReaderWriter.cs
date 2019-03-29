using Interfaces;
using Miscellaneous;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataAcquisitions.DAL
{
    internal class HDDXmlSerializator : HDDFileReaderWriterBase
    {

        private readonly HDDXmlSerializatorParameters _parameters;


        internal HDDXmlSerializator(HDDXmlSerializatorParameters parameter)
        {
            _parameters = parameter;
        }



        public override T ReadFromFile<T>(string fileNameAndPath, ToolNames toolName = null)
        {
            lock (_lockObject)
            {
                try
                {
                    if (!CheckFilePath(fileNameAndPath))
                    {
                        _parameters.Logger.MethodError($"File does not exists: {fileNameAndPath}");
                        return default(T);
                    }

                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    using (StreamReader sr = new StreamReader(fileNameAndPath))
                    {
                        return (T)serializer.Deserialize(sr);
                    }
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
        }



        public override bool WriteToFile<T>(T tobj, string fileNameAndPath)
        {
            lock (_lockObject)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = false;
                    settings.Encoding = new UnicodeEncoding(true, true);

                    using (StreamWriter sw = new StreamWriter(fileNameAndPath))
                    {
                        using (XmlWriter xw = XmlWriter.Create(sw, settings))
                        {
                            serializer.Serialize(xw, tobj);
                        }
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

    }

}
