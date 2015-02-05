using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Provides metadata about property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class PropertyAttribute : Attribute
    {
        /// <summary>
        /// User defined name of the property.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates instance and names property to <paramref name="name"/>.
        /// </summary>
        /// <param name="name">User defined name of the property.</param>
        public PropertyAttribute(string name)
        {
            Guard.NotNullOrEmpty(name, "name");
            Name = name;
        }
    }
}
