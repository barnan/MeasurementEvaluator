using Interfaces.Misc;
using System;
using System.Xml.Linq;

namespace Interfaces.BaseClasses
{

    public enum RelationsEnumValues
    {
        EQUAL = 0,
        NOTEQUAL = 1,
        LESS = 2,
        LESSOREQUAL = 3,
        GREATER = 4,
        GREATEROREQUAL = 5,
        ALLWAYS = 6,
    }


    public sealed class Relations : IXmlStorable
    {

        private const string XELEMENT_ATTRIBUTE_NAME = "Relation";

        public RelationsEnumValues Relation { get; private set; }

        public Func<bool, int, bool> Evaluation { get; set; }

        private string Text { get; set; }


        public Relations()
        {
        }

        private Relations(RelationsEnumValues relation, string text, Func<bool, int, bool> evaluation)
        {
            Relation = relation;
            Text = text;
            Evaluation = evaluation;
        }

        public static Relations EQUAL = new Relations(RelationsEnumValues.EQUAL, "==", (equality, compareResult) => equality);
        public static Relations NOTEQUAL = new Relations(RelationsEnumValues.NOTEQUAL, "!=", (equality, compareResult) => equality);
        public static Relations LESS = new Relations(RelationsEnumValues.LESS, ">", (equality, compareResult) => compareResult == -1);
        public static Relations LESSOREQUAL = new Relations(RelationsEnumValues.LESSOREQUAL, ">=", (equality, compareResult) => compareResult == 1 || equality);
        public static Relations GREATER = new Relations(RelationsEnumValues.GREATER, "<", (equality, compareResult) => compareResult == 1);
        public static Relations GREATEROREQUAL = new Relations(RelationsEnumValues.GREATEROREQUAL, "<=", (equality, compareResult) => compareResult == -1 || equality);
        public static Relations ALLWAYS = new Relations(RelationsEnumValues.ALLWAYS, "", (equality, compareResult) => true);

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator int(Relations relation)
        {
            return (int)relation.Relation;
        }


        public static explicit operator Relations(string val)
        {
            if ("==" == val)
            {
                return EQUAL;
            }
            if ("!=" == val)
            {
                return NOTEQUAL;
            }
            if (">" == val)
            {
                return LESS;
            }
            if (">=" == val)
            {
                return LESSOREQUAL;
            }
            if ("<" == val)
            {
                return GREATER;
            }
            if ("<=" == val)
            {
                return GREATEROREQUAL;
            }
            if ("" == val)
            {
                return ALLWAYS;
            }

            return null;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object other)
        {
            if (!(other is Relations otherRelation))
            {
                return false;
            }
            return Relation == otherRelation.Relation;
        }

        public XElement SaveToXml(XElement inputElement)
        {
            inputElement.SetAttributeValue(XELEMENT_ATTRIBUTE_NAME, Text);
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            var attrib = inputElement.Attribute(XELEMENT_ATTRIBUTE_NAME);
            if (attrib == null)
            {
                return false;
            }

            string attributeValue = attrib.Value;

            var element = (Relations)attributeValue;
            if (element == null)
            {
                return false;
            }

            Relation = element.Relation;
            Text = element.Text;
            Evaluation = element.Evaluation;

            return true;
        }
    }

}
