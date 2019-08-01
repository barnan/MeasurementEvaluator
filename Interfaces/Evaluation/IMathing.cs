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
        /// <param name="searchedConditionName">input specification name</param>
        /// <returns></returns>
        IEnumerable<string> GetMeasDataNames(string searchedConditionName);

        /// <summary>
        /// Gives the names of those reference values which are related to the given specification name
        /// </summary>
        /// <param name="searchedConditionName">input specification name</param>
        /// <returns></returns>
        string GetReferenceName(string searchedConditionName);
    }
}
