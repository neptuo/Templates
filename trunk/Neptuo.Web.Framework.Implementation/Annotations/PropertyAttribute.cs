using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;
using System.Reflection;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class PropertyAttribute : Attribute
    {
        public string Name { get; private set; }

        public PropertyAttribute()
        { }

        public PropertyAttribute(string name)
        {
            Name = name;
        }

        public static PropertyAttribute GetAttribute(PropertyInfo prop)
        {
            return ReflectionHelper.GetAttribute<PropertyAttribute>(prop);
        }
    }
}
