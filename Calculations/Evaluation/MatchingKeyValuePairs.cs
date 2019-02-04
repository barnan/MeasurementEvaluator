using System.Collections.Generic;

namespace Calculations.Evaluation
{
    public class MatchingKeyValuePairs
    {

        /// <summary>
        /// Specification condition name
        /// </summary>
        public string Key { get; }


        /// <summary>
        /// assignable measurement data names
        /// </summary>
        public IList<string> Values { get; }


        /// <summary>
        /// assignable reference name
        /// </summary>
        public string ReferenceName { get; }



        public MatchingKeyValuePairs()
        {
        }


        public MatchingKeyValuePairs(string key, IList<string> values, string referenceName)
        {
            Key = key;
            Values = values;
            ReferenceName = referenceName;
        }

    }
}
