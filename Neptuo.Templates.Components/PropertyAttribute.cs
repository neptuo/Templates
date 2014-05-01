using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Neptuo.Templates
{
    /// <summary>
    /// Provides metadata about property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class PropertyAttribute : Attribute
    {
        /// <summary>
        /// Property name.
        /// </summary>
        public string Name { get; private set; }

        public PropertyAttribute()
        { }

        public PropertyAttribute(string name)
        {
            Guard.NotNullOrEmpty(name, "name");
            Name = name;
        }
    }
}
