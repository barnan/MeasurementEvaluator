using Interfaces;
using Interfaces.DataAcquisition;
using Miscellaneous;
using System;
using System.IO;
using System.Text;
using System.Xml;
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


        public T ReadFromFile<T>(string fileNameAndPath, ToolNames toolName = null)
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


        public bool WriteToFile<T>(T tobj, string fileNameAndPath)
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
