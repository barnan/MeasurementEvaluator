using Frame.PluginLoader.Interfaces;
using Interfaces.DataAcquisition;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.HDDTabularMeasurementFileReaderWriter
{
    public class Factory : IPluginFactory
    {

        Dictionary<string, IHDDFileReaderWriter> _fileReaderDict = new Dictionary<string, IHDDFileReaderWriter>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDTabularMeasurementFileReaderWriter)))
            {
                if (!_fileReaderDict.ContainsKey(name))
                {
                    Parameters param = new Parameters();
                    if (param.Load(name))
                    {
                        HDDTabularMeasurementFileReaderWriter instance = new HDDTabularMeasurementFileReaderWriter(param);
                        _fileReaderDict.Add(name, instance);
                        return instance;
                    }
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
