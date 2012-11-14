using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BuilderAttribute : Attribute
    {
        public Type Type { get; set; }

        public BuilderAttribute(Type type)
        {
            Type = type;
        }
    }
}
