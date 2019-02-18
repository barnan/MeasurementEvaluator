namespace Interfaces.Misc
{
    public interface INamed
    {
        string Name { get; }
    }


    public interface INamedHandler : INamed
    {
        new string Name { get; set; }
    }

}
