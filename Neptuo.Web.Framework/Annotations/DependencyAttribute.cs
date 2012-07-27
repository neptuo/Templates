using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DependencyAttribute : Attribute
    {


        public static DependencyAttribute GetAttribute(PropertyInfo prop)
        {
            return ReflectionHelper.GetAttribute<DependencyAttribute>(prop);
        }
    }
}
