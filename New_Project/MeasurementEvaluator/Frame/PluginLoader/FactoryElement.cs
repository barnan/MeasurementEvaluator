using FrameInterfaces;

namespace Frame.PluginLoader
{
    internal class FactoryElement
    {
        internal IPluginFactory Factory { get; set; }
        internal string AssemblyName { get; set; }
    }
}
