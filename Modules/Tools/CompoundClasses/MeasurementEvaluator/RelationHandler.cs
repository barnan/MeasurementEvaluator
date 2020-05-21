using BaseClasses;
using Interfaces.Misc;
using System;
using System.Xml.Linq;

namespace Interfaces.BaseClasses
{

    public sealed class RelationHandler : IXmlStorable
    {

        private const string XELEMENT_ATTRIBUTE_NAME = "Relation";

        public Relations Relation { get; private set; }

        public Func<bool, int, bool> Evaluation { get; set; }

        private string Text { get; set; }


        public RelationHandler()
        {
        }

        private RelationHandler(Relations relation, string text, Func<bool, int, bool> evaluation)
        {
            Relation = relation;
            Text = text;
            Evaluation = evaluation;
        }

        public static RelationHandler EQUAL = new RelationHandler(Relations.EQUAL, "==", (equality, compareResult) => equality);

        public static RelationHandler NOTEQUAL = new RelationHandler(Relations.NOTEQUAL, "!=", (equality, compareResult) => equality);

        public static RelationHandler LESS = new RelationHandler(Relations.LESS, ">", (equality, compareResult) => compareResult == -1);

        public static RelationHandler LESSOREQUAL = new RelationHandler(Relations.LESSOREQUAL, ">=", (equality, compareResult) => compareResult == 1 || equality);

        public static RelationHandler GREATER = new RelationHandler(Relations.GREATER, "<", (equality, compareResult) => compareResult == 1);

        public static RelationHandler GREATEROREQUAL = new RelationHandler(Relations.GREATEROREQUAL, "<=", (equality, compareResult) => compareResult == -1 || equality);

        //public static RelationHandler ALLWAYS = new RelationHandler(Relations.ALLWAYS, "ALLWAYS", (equality, compareResult) => true);

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator int(RelationHandler relation)
        {
            return (int)relation.Relation;
        }


        public static explicit operator RelationHandler(string val)
        {
            if (EQUAL.Text == val)
            {
                return EQUAL;
            }
            if (NOTEQUAL.Text == val)
            {
                return NOTEQUAL;
            }
            if (LESS.Text == val)
            {
                return LESS;
            }
            if (LESSOREQUAL.Text == val)
            {
                return LESSOREQUAL;
            }
            if (GREATER.Text == val)
            {
                return GREATER;
            }
            if (GREATEROREQUAL.Text == val)
            {
                return GREATEROREQUAL;
            }
            //if (ALLWAYS.Text == val)
            //{
            //    return ALLWAYS;
            //}

            return null;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object other)
        {
            if (!(other is RelationHandler otherRelation))
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

            var element = (RelationHandler)attributeValue;
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
