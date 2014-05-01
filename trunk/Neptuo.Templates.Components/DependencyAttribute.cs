using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates
{
    /// <summary>
    /// Marks property for dependecy injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DependencyAttribute : Attribute
    {
        /// <summary>
        /// Dependency registration name.
        /// </summary>
        public string Name { get; private set; }

        public DependencyAttribute()
        { }

        public DependencyAttribute(string name)
        {
            Guard.NotNullOrEmpty(name, "name");
            Name = name;
        }
    }
}
