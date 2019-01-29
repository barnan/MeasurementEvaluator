using Interfaces.Misc;
using System.Collections.Generic;

namespace Interfaces.Evaluation
{
    public interface IMathing : IInitializable
    {

        IEnumerable<string> GetMeasDataNames(string specificationName);

        string GetreferenceName(string specificationName);

    }
}
