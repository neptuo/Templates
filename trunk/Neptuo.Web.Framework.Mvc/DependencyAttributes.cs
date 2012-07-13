using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace Neptuo.Web.Mvc.ViewEngine
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DependencyAttribute : Attribute
    { }

    public static class DependencyAttributes
    {
        public static PropertyInfo[] GetProperties(Type type)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (prop.GetCustomAttributes(typeof(DependencyAttribute), true).Length != 0)
                    result.Add(prop);
            }

            return result.ToArray();
        }
    }
}