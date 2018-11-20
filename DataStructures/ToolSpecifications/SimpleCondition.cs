using Interfaces;
using Interfaces.ToolSpecifications;
using System.Collections.Generic;

namespace DataStructures.ToolSpecifications
{
    public class SimpleCondition : ConditionBase<double>, ISimpleCondition<double>
    {
        public string Dimension { get; }

        public Relations ValidIf { get; }

        public double ValidIf_Value { get; }



        public SimpleCondition(double value, Relations relation, bool valid, IComparer<double> comparer, string dimension, Relations validIf, double validIf_Value)
            : base(value, relation, valid, comparer)
        {
            Dimension = dimension;
            ValidIf = validIf;
            ValidIf_Value = validIf_Value;
        }


    }
}
