using PluginLoading.Interfaces;
using System;

namespace DataAcquisitions.DataCollector
{
    public class Factory : IPluginFactory
    {

        public object Create(Type t, string title)
        {
            if (t.IsAssignableFrom(typeof(DataCollector)))
            {
                DataCollectorParameters param = new DataCollectorParameters();

                return new DataCollector();
            }
        }

    }
}
