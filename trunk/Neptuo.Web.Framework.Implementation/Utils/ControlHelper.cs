using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Neptuo.Web.Framework.Annotations;

namespace Neptuo.Web.Framework.Utils
{
    public class ControlHelper
    {
        public static Dictionary<string, PropertyInfo> GetProperties(Type control)
        {
            Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo property in control.GetProperties())
            {
                string propertyName = property.Name;
                PropertyAttribute attribute = ReflectionHelper.GetAttribute<PropertyAttribute>(property);
                if (attribute != null)
                    propertyName = attribute.Name ?? propertyName;

                result[propertyName] = property;
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
                    PropertyInfo property = control.GetProperties().LastOrDefault(p => p.Name == defaultPropertyAttribute.Name);
                    if (property != null)
                        return property;
                }
            }

            return null;
        }
    }
}
