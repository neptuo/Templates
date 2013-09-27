using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DependencyAttribute : Attribute
    {
        public string Name { get; private set; }

        public DependencyAttribute()
        { }

        public DependencyAttribute(string name)
        {
            Name = name;
        }
    }
}
