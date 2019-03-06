using Interfaces.Misc;
using System.Collections.Generic;

namespace Interfaces.Evaluation
{
    // The matcher gives back to the evaluation the coherent data for the evaulation
    public interface IMathing : IInitializable
    {
        /// <summary>
        /// Gives the names of those measurement datas which are related to the given specification name
        /// </summary>
        /// <param name="specificationName">input specification name</param>
        /// <returns></returns>
        IEnumerable<string> GetMeasDataNames(string specificationName);

        /// <summary>
        /// Gives the names of those reference values which are related to the given specification name
        /// </summary>
        /// <param name="specificationName">input specification name</param>
        /// <returns></returns>
        string GetreferenceName(string specificationName);
    }
}
