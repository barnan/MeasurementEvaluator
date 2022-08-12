
namespace Interfaces.Misc
{
    public interface INamedContent <T> : INamed
    {
        T Content { get; }
    }
}
