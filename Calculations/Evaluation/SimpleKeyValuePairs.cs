using System.Collections.Generic;

namespace Calculations.Evaluation
{
    public class SimpleKeyValuePairs
    {

        /// <summary>
        /// Specification condition name
        /// </summary>
        public string Key { get; }


        /// <summary>
        /// assignable measurement data names
        /// </summary>
        public IEnumerable<string> Values { get; }


        /// <summary>
        /// assignable reference name
        /// </summary>
        public string ReferenceName { get; }



        public SimpleKeyValuePairs()
        {
        }


        public SimpleKeyValuePairs(string key, IEnumerable<string> values, string referenceName)
        {
            Key = key;
            Values = values;
            ReferenceName = referenceName;
        }

    }
}
