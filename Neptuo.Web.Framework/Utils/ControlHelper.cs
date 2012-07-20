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
            object[] attributes = control.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
            if (attributes.Length > 0)
            {
                DefaultPropertyAttribute defaultPropertyAttribute = attributes[0] as DefaultPropertyAttribute;
                if (defaultPropertyAttribute != null && defaultPropertyAttribute.Name != null)
                {
                    PropertyInfo property = control.GetProperty(defaultPropertyAttribute.Name);
                    if (property != null)
                        return property;
                }
            }

            return null;
        }
    }
}
