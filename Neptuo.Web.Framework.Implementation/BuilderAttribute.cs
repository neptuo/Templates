using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BuilderAttribute : Attribute
    {
        public Type[] Types { get; set; }

        public BuilderAttribute(params Type[] types)
        {
            Types = types;
        }
    }
}
