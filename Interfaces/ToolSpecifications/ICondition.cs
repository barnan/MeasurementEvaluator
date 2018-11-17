﻿namespace Measurement_Evaluator.Interfaces.ToolSpecifications
{

    public enum Relations : byte
    {
        EQUAL = 0,
        NOTEQUAL,
        LESS,
        LESSOREQUAL,
        GREATER,
        GREATEROREQUAL
    }

    public enum RELATIVEORABSOLUTE : byte
    {
        ABSOLUTE = 0,
        RELATIVE = 1
    }



    public interface ICondition
    {
        /// <summary>
        /// value of the condition
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// Relation in the condition       e.g.   <   >   ==   >=   <=
        /// </summary>
        Relations ConditionRelation { get; set; }

        /// <summary>
        /// Validity of the condition -> if false, the condition does not work
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Name of the condition
        /// </summary>
        string Name { get; set; }
    }
}
