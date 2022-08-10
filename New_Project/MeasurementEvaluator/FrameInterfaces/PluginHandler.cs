
namespace FrameInterfaces
{
    public interface IPluginFactory
    {
        object Create(Type t, string name);
    }
}
