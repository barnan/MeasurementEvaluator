using System;

namespace PluginLoading.Interfaces
{

    public interface IPluginFactory
    {
        object Create(Type t);
    }

    public interface IPluginLoading
    {
        T CreateInstance<T>();
    }

}
