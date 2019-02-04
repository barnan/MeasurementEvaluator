using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DataAcquisition.DAL
{
    public class HdDdXmlSerializator : HDDFileReaderWriterBase
    {


        public override T ReadFromFile<T>(string fileNameAndPath, string toolName = null)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                T tobj;
                using (StreamReader sr = new StreamReader(fileNameAndPath))
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



        public override bool WriteToFile<T>(T tobj, string fileNameAndPath)
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
