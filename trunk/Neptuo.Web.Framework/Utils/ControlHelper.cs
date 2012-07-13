using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Neptuo.Web.Framework.Utils
{
    public class ControlHelper
    {
        public static Dictionary<string, PropertyInfo> GetProperties(Type control)
        {
            Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo property in control.GetProperties())
            {
                result.Add(property.Name, property);
            }

            return result;
        }

        public static PropertyInfo GetDefaultProperty(Type control)
        {
            foreach (KeyValuePair<string, PropertyInfo> property in GetProperties(control))
            {
                object[] attributes = property.Value.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
                if (attributes.Length == 1)
                    return property.Value;
            }

            return null;
        }
    }
}
