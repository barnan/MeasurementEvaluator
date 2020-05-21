using System;

namespace MyFrameWork.Interfaces
{
    public interface IPluginFactory
    {
        object Create(Type t, string name);
    }


    public interface IRunable
    {
        void Run();
    }
}
