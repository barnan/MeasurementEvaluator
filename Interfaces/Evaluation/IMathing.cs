using Interfaces.Misc;

namespace Interfaces.Evaluation
{
    public interface IMathing : IInitializable
    {

        string GetSpecification(string measuredDataName);

        string Getreference(string measuredDataName);

    }
}
