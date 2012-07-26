using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Neptuo.Web.Framework.Utils
{
    public static class TypeConverter
    {
        public static bool CanConvert(Type target)
        {
            if (target == typeof(String))
                return true;

            if (target.IsEnum)
                return true;

            if (target == typeof(bool) || target == typeof(int))
                return true;

            return false;
        }

        public static object Convert(string value, Type target)
        {
            if (target.IsGenericType && target.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (String.IsNullOrEmpty(value))
                    return null;

                target = target.GetGenericArguments()[0];
            }

            if (String.IsNullOrEmpty(value))
                return GetDefaultValue(target);

            if (target == typeof(String))
                return value;

            if (target.IsEnum)
            {
                try
                {
                    object enumValue = Enum.Parse(target, value);
                    if (enumValue != null)
                        return enumValue;
                }
                catch (ArgumentException)
                {
                    return GetDefaultValue(target);
                }
            }

            if (target == typeof(bool))
                return Convert(value, Boolean.TryParse, false);

            if (target == typeof(int))
                return Convert(value, Int32.TryParse, 0);

            //TypeDescriptor.GetConverter(typeof(GradientColor))
            throw new ApplicationException("Unsupported type!");
        }

        private static object Convert<T>(string value, Func<string, T, bool> func, T defaultValue)
        {
            T converted = defaultValue;
            if (func(value, out converted))
                return converted;

            return defaultValue;
        }

        public static object GetDefaultValue(Type target)
        {
            Type nullable = Nullable.GetUnderlyingType(target);
            if (nullable != null)
                return null;

            if (target.IsValueType)
                return Activator.CreateInstance(target);

            if(target == typeof(String))
                return null;

            if (target == typeof(bool))
                return false;

            if (target == typeof(int) || target == typeof(double) || target == typeof(float) || target == typeof(byte) || target == typeof(decimal))
                return 0;

            throw new ApplicationException("Unsupported type!");
        }
    }
}
