﻿using Interfaces.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Miscellaneous
{
    public static class SerializationExtension_Load
    {
        public static void TryLoad<T, TV>(this IXmlStorable storable, Expression<Func<T, TV>> expr, XElement inputElement, string name = null)
        {
            MemberExpression memberExpression = expr.Body as MemberExpression;
            PropertyInfo propInfo = memberExpression.Member as PropertyInfo;
            storable.TryLoad(inputElement, name ?? propInfo.Name);
        }


        public static void TryLoad(this IXmlStorable storable, XElement inputElement, string name)
        {
            PropertyInfo[] propInfos = storable.GetType().GetProperties();


            PropertyInfo propInfo = propInfos.FirstOrDefault(p => p.Name == name);

            if (propInfo.PropertyType.IsInterface && typeof(IXmlStorable).IsAssignableFrom(propInfo.PropertyType))
            {
                IXmlStorable xmlStorableChild = propInfo.GetValue(storable) as IXmlStorable;
                xmlStorableChild.LoadFromXml(inputElement);
            }
            else
            {
                XElement propElement = inputElement.Element(name);
                Type type = propInfo.PropertyType;
                var value = Load(type, propElement, name);
                propInfo.SetValue(storable, value);
            }

        }


        public static TV TryLoad<TV>(ref TV inputobj, XElement inputElement, string name)
        {
            Type inputType = typeof(TV);
            if (inputType == typeof(object))
            {
                inputType = inputobj.GetType();
            }

            try
            {
                inputobj = (TV)Load(inputType, inputElement, name);
            }
            catch (Exception ex)
            {
                inputobj = default(TV);
            }
            return inputobj;
        }


        private static object Load(Type inputType, XElement inputElement, string name)
        {
            string serializableName = name;
            string attributeValue = null;
            if (serializableName == null)
            {
                serializableName = inputType.ToString();
            }

            if (inputType.IsInterface)
            {
                serializableName = inputElement.Name.LocalName;
                attributeValue = inputElement.Attribute("Type").Value;
            }

            if (typeof(Type).IsAssignableFrom(inputType))
            {
                string assemblyName = inputElement.Attribute("Assembly").Value;
                if (inputElement.Value.Contains("String"))
                {
                    var variable = Activator.CreateInstance(Type.GetType(inputElement.Value), "".ToCharArray());
                    return variable.GetType();
                }
                else
                {
                    var variable = Activator.CreateInstance(Type.GetType(inputElement.Value));
                    return variable.GetType();
                }
            }

            if (typeof(string).IsAssignableFrom(inputType))
            {
                return inputElement.Value;
            }

            if (inputType.IsValueType && inputType.IsPrimitive)
            {
                try
                {
                    return TypeDescriptor.GetConverter(inputType).ConvertFromString(inputElement.Value);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            if (typeof(IXmlStorable).IsAssignableFrom(inputType))
            {
                var storableObject = (IXmlStorable)Activator.CreateInstance(Type.GetType(attributeValue ?? serializableName));
                storableObject.LoadFromXml(inputElement);
                return storableObject;
            }

            if (typeof(IList).IsAssignableFrom(inputType))
            {
                Type listGenericType = typeof(List<>);
                Type listType = listGenericType.MakeGenericType(inputType.GetGenericArguments()[0]);
                ConstructorInfo ci = listType.GetConstructor(new Type[] { });
                var list = ci.Invoke(new object[] { }) as IList;

                Type elementType = inputType.GetGenericArguments()[0];

                if (inputType.GetGenericArguments().Length > 0 && elementType.IsValueType)
                {
                    string[] items = inputElement.Value.Split(new char[] { ',', ';' });
                    foreach (string item in items)
                    {
                        list.Add(Convert.ChangeType(item, elementType));
                    }
                    return list;
                }
                else
                {
                    var xlistItems = inputElement.Elements();
                    foreach (var xItem in xlistItems)
                    {
                        string itemName = xItem.Name.LocalName;
                        string attribContent = xItem.Attribute("Type").Value;


                        var loadedListElement = Load(elementType, xItem, itemName);
                        list.Add(Convert.ChangeType(loadedListElement, Type.GetType(attribContent ?? inputType.Name)));
                    }

                }
                return list;
            }

            if (inputType.IsSerializable)
            {
                var xmlSerializer = new XmlSerializer(inputType);
                return xmlSerializer.Deserialize(inputElement.CreateReader());

            }



            return null;

        }

    }
}
