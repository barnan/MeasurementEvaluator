namespace Interfaces.Misc
{
    public interface INamedObject
    {
        string Name { get; }
    }


    public interface INamedObjectHandler : INamedObject
    {
        new string Name { get; set; }
    }

}
