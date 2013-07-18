using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Neptuo.Templates
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
    }
}
