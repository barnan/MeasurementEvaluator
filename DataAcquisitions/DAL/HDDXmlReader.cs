using Interfaces.DataAcquisition;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataAcquisition.DAL
{
    public class HDDdXmlReader : MeasurementDataFileBase, IHDDXmlReader
    {
        private T DeserializeObject<T>(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                T tobj;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    tobj = (T)serializer.Deserialize(sr);
                }

                return tobj;
            }
            catch (Exception)
            {
                return default(T);
            }
        }



        private bool SerializeObject<T>(T tobj, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = false;
                settings.Encoding = new UnicodeEncoding(true, true);

                using (StreamWriter sw = new StreamWriter(filePath))
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

        public override T ReadFile<T>(string fileNameAndPath, string toolName = null)
        {
            return DeserializeObject<T>(fileNameAndPath);
        }


        public override bool WriteFile<T>(T obj, string fileNameAndPath)
        {
            return SerializeObject(obj, fileNameAndPath);
        }


    }
}
