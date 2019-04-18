using Interfaces.Misc;
using System;
using System.Collections;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Miscellaneous
{
    public static class SerializationExtension_Save
    {

        public static void TrySave<T, TV>(this T storable, Expression<Func<T, TV>> expr, XElement element, string name = null)
        {
            MemberExpression memberExpression = expr.Body as MemberExpression;
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            object value = propertyInfo.GetValue(storable, null);

            TrySave<TV>((TV)value, element, name ?? propertyInfo.Name);
        }


        public static void TrySave<TV>(this IXmlStorable storable, TV inputobj, XElement element, string name)
        {
            TrySave<TV>(inputobj, element, name);
        }


        public static void TrySave<TV>(TV inputobj, XElement inputElement, string name)
        {
            if (Equals(inputobj, null))
            {
                return;
            }

            Type inputType = typeof(TV);
            if (inputType == typeof(object))
            {
                inputType = inputobj.GetType();
            }

            Save(inputobj, inputType, inputElement, name);
        }


        private static void Save(object inputobj, Type inputType, XElement inputElement, string name)
        {
            string serializableName = name;
            XAttribute attribute = null;

            IList listVersion = inputobj as IList;
            IDictionary dictVersion = inputobj as IDictionary;
            IXmlStorable xmlstorableVerison = inputobj as IXmlStorable;

            if (serializableName == null)
            {
                if (listVersion != null)
                {
                    serializableName = "List";
                }
                else
                {
                    serializableName = inputobj.GetType().ToString();
                }
            }

            if (inputType == typeof(Type))
            {
                attribute = new XAttribute("Assembly", ((Type)inputobj).AssemblyQualifiedName);
            }
            else
            {
                attribute = new XAttribute("Assembly", inputobj.GetType().AssemblyQualifiedName);
            }

            // type:
            if (inputType == typeof(Type))
            {
                XElement typeElement = new XElement(name, inputobj.ToString());
                typeElement.Add(attribute);
                inputElement.Add(typeElement);
                return;
            }

            // string:
            if (typeof(string).IsAssignableFrom(inputType))
            {
                XElement stringElement = new XElement(name, inputobj);
                stringElement.Add(attribute);
                inputElement.Add(stringElement);
                return;
            }

            // value type:
            if (inputType.IsValueType && inputType.IsPrimitive)
            {
                XElement valueElement = new XElement(name, inputobj);
                valueElement.Add(attribute);
                inputElement.Add(valueElement);
                return;
            }


            //ISerializable serializableVersion = inputobj as ISerializable;


            //xml sotrable:
            if (xmlstorableVerison != null)
            {
                XElement xmlStorableXElement = new XElement(serializableName);
                xmlstorableVerison.SaveToXml(xmlStorableXElement);
                //if (attribute != null)
                //{
                xmlStorableXElement.Add(attribute);
                //}
                inputElement.Add(xmlStorableXElement);
                return;
            }

            // list:
            if (listVersion != null)
            {
                Type[] itemTypes = inputType.GetGenericArguments();

                if (itemTypes.Length > 0 && itemTypes[0].IsValueType)
                {
                    // value types in the list:

                    XElement valueListXElement = new XElement(serializableName);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < listVersion.Count - 1; i++)
                    {
                        sb.Append(listVersion[i]);
                        sb.Append(",");
                    }
                    sb.Append(listVersion[listVersion.Count - 1]);
                    valueListXElement.Value = sb.ToString();
                    inputElement.Add(valueListXElement);
                }
                else
                {
                    // reference types in the list:

                    XElement refListXElement = new XElement(serializableName);
                    foreach (var item in listVersion)
                    {
                        //if (attribute != null)
                        //{
                        //refListXElement.Add(attribute);
                        //}

                        Save(item, itemTypes[0], refListXElement, null);
                    }
                    refListXElement.Add(attribute);
                    inputElement.Add(refListXElement);
                }
                return;
            }

            if (dictVersion != null)
            {
                if (inputType.GetGenericArguments().Length > 0)
                {

                }
            }

            // serializable:
            //if (serializableVersion != null)
            if (inputobj.GetType().IsSerializable)
            {
                using (var memoryStream = new MemoryStream())
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(inputobj.GetType());
                    xmlSerializer.Serialize(streamWriter, inputobj);
                    XElement serializedXElement = XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                    serializedXElement.Add(attribute);
                    inputElement.Add(serializedXElement);
                }
            }
        }
    }
}
