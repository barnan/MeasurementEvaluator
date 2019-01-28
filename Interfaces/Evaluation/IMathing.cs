using Interfaces.Misc;
using System.Collections.Generic;

namespace Interfaces.Evaluation
{
    public interface IMathing : IInitializable
    {

        IEnumerable<string> GetSpecification(string measuredDataName);

        IEnumerable<string> Getreference(string measuredDataName);

    }
}
