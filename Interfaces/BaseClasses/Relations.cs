using Interfaces.Misc;
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

        //public string Name { get; private set; }
        //public int Value { get; private set; }

        public RelationsEnumValues Relation { get; private set; }

        private string Text { get; set; }


        public Relations()
        {
        }

        private Relations(RelationsEnumValues relation, string text)
        {
            //Name = name;
            //Value = value;
            Relation = relation;
            Text = text;
        }

        //private Relations(string name, int value, string text)
        //{
        //    Name = name;
        //    Value = value;
        //    Text = text;
        //}

        //public Relations()
        //{
        //}

        //public class RelationsEnumValues
        //{
        //    public const int EQUAL = 0;
        //    public const int NOTEQUAL = 1;
        //    public const int LESS = 2;
        //    public const int LESSOREQUAL = 3;
        //    public const int GREATER = 4;
        //    public const int GREATEROREQUAL = 5;
        //    public const int ALLWAYS = 6;
        //}

        //public static Relations EQUAL = new Relations(nameof(EQUAL), RelationsEnumValues.EQUAL, "==");
        //public static Relations NOTEQUAL = new Relations(nameof(NOTEQUAL), RelationsEnumValues.NOTEQUAL, "!=");
        //public static Relations LESS = new Relations(nameof(LESS), RelationsEnumValues.LESS, ">");
        //public static Relations LESSOREQUAL = new Relations(nameof(LESSOREQUAL), RelationsEnumValues.LESSOREQUAL, ">=");
        //public static Relations GREATER = new Relations(nameof(GREATER), RelationsEnumValues.GREATER, "<");
        //public static Relations GREATEROREQUAL = new Relations(nameof(GREATEROREQUAL), RelationsEnumValues.GREATEROREQUAL, "<=");
        //public static Relations ALLWAYS = new Relations(nameof(ALLWAYS), RelationsEnumValues.ALLWAYS, "");

        public static Relations EQUAL = new Relations(RelationsEnumValues.EQUAL, "==");
        public static Relations NOTEQUAL = new Relations(RelationsEnumValues.NOTEQUAL, "!=");
        public static Relations LESS = new Relations(RelationsEnumValues.LESS, ">");
        public static Relations LESSOREQUAL = new Relations(RelationsEnumValues.LESSOREQUAL, ">=");
        public static Relations GREATER = new Relations(RelationsEnumValues.GREATER, "<");
        public static Relations GREATEROREQUAL = new Relations(RelationsEnumValues.GREATEROREQUAL, "<=");
        public static Relations ALLWAYS = new Relations(RelationsEnumValues.ALLWAYS, "");

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

            //Name = element.Name;
            //Value = element.Value;
            Relation = element.Relation;
            Text = element.Text;
            return true;
        }
    }

}
