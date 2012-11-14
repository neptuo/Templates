using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ComponentAttribute : Attribute
    {
        public string Name { get; set; }

        public ComponentAttribute()
        { }

        public ComponentAttribute(string name)
        {
            Name = name;
        }
    }
}
