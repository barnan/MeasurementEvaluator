using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.Misc;
using System;
using System.IO;
using System.Xml.Linq;

namespace DataAcquisitions.HDDXmlSerializator
{
    internal abstract class HDDXmlSerializator : IHDDFileReaderWriter
    {

        private readonly HDDXmlSerializatorParameters _parameters;
        private readonly object _lockObject = new object();


        internal HDDXmlSerializator(HDDXmlSerializatorParameters parameter)
        {
            _parameters = parameter;
        }


        public abstract object ReadFromFile(string fileNameAndPath, ToolNames toolName = null);




        public bool WriteToFile(object tobj, string fileNameAndPath)
        {
            lock (_lockObject)
            {
                try
                {
                    if (tobj is IXmlStorable storable)
                    {
                        Type type = tobj.GetType();

                        XElement element = new XElement(type.FullName);
                        storable.SaveToXml(element);
                        element.Save(fileNameAndPath);
                        return true;
                    }

                    _parameters.Logger.Error($"Received object is not {nameof(IXmlStorable)}");
                    return false;
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
