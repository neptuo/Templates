using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;
using System.Reflection;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ControlPropertyAttribute : Attribute
    {
        public string Namespace { get; private set; }

        public string Name { get; private set; }

        public ControlPropertyAttribute()
        {

        }

        public ControlPropertyAttribute(string name)
        {
            Name = name;
        }

        public ControlPropertyAttribute(string newNamespace, string name)
        {
            Namespace = newNamespace;
            Name = name;
        }

        public static ControlPropertyAttribute GetAttribute(PropertyInfo prop)
        {
            return ReflectionHelper.GetAttribute<ControlPropertyAttribute>(prop);
        }
    }
}
