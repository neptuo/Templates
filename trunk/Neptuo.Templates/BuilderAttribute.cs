using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class BuilderAttribute : Attribute
    {
        public Type Type { get; set; }

        public BuilderAttribute(Type type)
        {
            Type = type;
        }
    }
}
