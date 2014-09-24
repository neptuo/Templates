using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation
{
    /// <summary>
    /// Provides metadata about control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ComponentAttribute : Attribute
    {
        /// <summary>
        /// Name of control for standart registration.
        /// </summary>
        public string Name { get; set; }

        public ComponentAttribute()
        { }

        public ComponentAttribute(string name)
        {
            Guard.NotNullOrEmpty(name, "name");
            Name = name;
        }
    }
}
