using System.ComponentModel;
using System.Reflection;

namespace Utils
{
    public static class EnumExtension
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attribs =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            
            if (attribs != null && attribs.Any())
            {
                return attribs.First().Description;
            }

            return value.ToString();
        }


        public static Enum GetEnumValueFromDescription(Type enumToInvestigate, string descriptionToMatch)
        {
            Array enumValues = Enum.GetValues(enumToInvestigate);
            foreach (Enum enumValue in enumValues)
            {
                if (enumValue.GetEnumDescription() == descriptionToMatch)
                {
                    return enumValue;
                }
            }

            throw new InvalidOperationException($"The given description ({descriptionToMatch}) was not found in {enumToInvestigate}");
        }


        /// <summary>
        /// First -> examines the description.
        /// Second -> examines the tostring form (If no match was found in the first step)
        /// </summary>
        public static Enum GetEnumValueFromText(Type enumToInvestigate, string text)
        {
            Array enumValues = Enum.GetValues(enumToInvestigate);

            foreach (Enum enumValue in enumValues)
            {
                if (enumValue.GetEnumDescription() == text || enumValue.ToString() == text)
                {
                    return enumValue;
                }
            }

            throw new InvalidOperationException($"The given description or name ({text}) was not found in {enumToInvestigate}");
        }

    }
}