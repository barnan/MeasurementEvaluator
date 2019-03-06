using System;

namespace PluginLoading.Interfaces
{

    public interface IPluginFactory
    {
        object Create(Type t, string name);
    }

}
