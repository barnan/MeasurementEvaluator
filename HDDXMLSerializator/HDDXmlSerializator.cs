﻿using Interfaces;
using Interfaces.DataAcquisition;
using Interfaces.Misc;
using System;
using System.IO;
using System.Xml.Linq;

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
                    object createdObj = Activator.CreateInstance(Type.GetType(readElement.Name.LocalName));
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
