using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.HDDXmlSerializator
{
    public class Factory : IPluginFactory
    {

        Dictionary<string, IHDDFileReaderWriter> _fileReaderDict = new Dictionary<string, IHDDFileReaderWriter>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDXmlSerializator)))
            {
                if (!_fileReaderDict.ContainsKey(name))
                {
                    HDDXmlSerializatorParameters param = new HDDXmlSerializatorParameters();
                    HDDXmlSerializator instance = new HDDXmlSerializator(param);
                    _fileReaderDict.Add(name, instance);
                    return instance;
                }
                else
                {
                    return _fileReaderDict[name];
                }
            }
            return null;
        }
    }
}
