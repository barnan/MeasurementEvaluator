using Interfaces.Misc;
using System.Xml.Linq;

namespace Interfaces.BaseClasses
{
    public sealed class Relations : IXmlStorable
    {

        private const string XELEMENT_ATTRIBUTE_NAME = "Value";

        public string Name { get; private set; }
        public int Value { get; private set; }
        private string Text { get; }

        private Relations(string name, int value, string text)
        {
            Name = name;
            Value = value;
            Text = text;
        }

        public Relations()
        {
        }

        public class RelationsEnumValues
        {
            public const int EQUAL = 0;
            public const int NOTEQUAL = 1;
            public const int LESS = 2;
            public const int LESSOREQUAL = 3;
            public const int GREATER = 4;
            public const int GREATEROREQUAL = 5;
            public const int ALLWAYS = 6;
        }

        public static Relations EQUAL = new Relations(nameof(EQUAL), RelationsEnumValues.EQUAL, "==");
        public static Relations NOTEQUAL = new Relations(nameof(NOTEQUAL), RelationsEnumValues.NOTEQUAL, "!=");
        public static Relations LESS = new Relations(nameof(LESS), RelationsEnumValues.LESS, ">");
        public static Relations LESSOREQUAL = new Relations(nameof(LESSOREQUAL), RelationsEnumValues.LESSOREQUAL, ">=");
        public static Relations GREATER = new Relations(nameof(GREATER), RelationsEnumValues.GREATER, "<");
        public static Relations GREATEROREQUAL = new Relations(nameof(GREATEROREQUAL), RelationsEnumValues.GREATEROREQUAL, "<=");
        public static Relations ALLWAYS = new Relations(nameof(ALLWAYS), RelationsEnumValues.ALLWAYS, "");

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator int(Relations rel)
        {
            return rel.Value;
        }


        public static explicit operator Relations(string val)
        {
            if (EQUAL.ToString() == val)
            {
                return EQUAL;
            }
            if (NOTEQUAL.ToString() == val)
            {
                return NOTEQUAL;
            }
            if (LESS.ToString() == val)
            {
                return LESS;
            }
            if (LESSOREQUAL.ToString() == val)
            {
                return LESSOREQUAL;
            }
            if (GREATER.ToString() == val)
            {
                return GREATER;
            }
            if (GREATEROREQUAL.ToString() == val)
            {
                return GREATEROREQUAL;
            }
            if (ALLWAYS.ToString() == val)
            {
                return ALLWAYS;
            }

            return null;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object other)
        {
            Relations otherRelation = other as Relations;
            if (ReferenceEquals(null, otherRelation))
            {
                return false;
            }
            return Value == otherRelation.Value;
        }

        public XElement SaveToXml(XElement inputElement)
        {
            inputElement.SetAttributeValue(XELEMENT_ATTRIBUTE_NAME, Name);
            return inputElement;
        }

        public bool LoadFromXml(XElement inputElement)
        {
            string attributeValue = inputElement.Attribute(XELEMENT_ATTRIBUTE_NAME)?.Value;

            var element = (Relations)attributeValue;
            if (element == null)
            {
                return false;
            }

            Name = element.Name;
            Value = element.Value;
            return true;
        }
    }

}
