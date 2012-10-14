using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ControlAttribute : Attribute
    {
        public string Name { get; set; }

        public ControlAttribute()
        { }

        public ControlAttribute(string name)
        {
            Name = name;
        }

        public static ControlAttribute GetAttribute(Type type)
        {
            return ReflectionHelper.GetAttribute<ControlAttribute>(type);
        }
    }
}
