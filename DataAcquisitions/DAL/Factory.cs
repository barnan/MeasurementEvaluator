using Frame.PluginLoader.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAcquisitions.DAL
{
    public class Factory : IPluginFactory
    {

        Dictionary<string, HDDFileReaderWriterBase> _fileReaderDict = new Dictionary<string, HDDFileReaderWriterBase>();


        public object Create(Type t, string name)
        {
            if (t.IsAssignableFrom(typeof(HDDFileReaderWriterBase)))
            {
                if (!_fileReaderDict.ContainsKey(name))
                {
                    //EvaluationParameters param = new EvaluationParameters();
                    //if (param.Load(name))
                    //{
                    //    HDDFileReaderWriterBase instance = new HDDFileReaderWriterBase(param);
                    //    _dataCollectorDict.Add(name, instance);
                    //}
                }
                return _fileReaderDict[name];
            }
            return null;
        }
    }
}
