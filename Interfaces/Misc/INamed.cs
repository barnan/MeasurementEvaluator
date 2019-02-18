namespace Interfaces.Misc
{
    public interface INamed
    {
        string Name { get; }
    }


    public interface INamedObjectHandler : INamed
    {
        new string Name { get; set; }
    }

}
