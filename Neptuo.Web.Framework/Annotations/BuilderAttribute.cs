using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BuilderAttribute : Attribute
    {
        public Type BuilderType { get; protected set; }

        public BuilderAttribute(Type builderType)
        {
            BuilderType = builderType;
        }

        public static BuilderAttribute GetAttribute(Type type)
        {
            return ReflectionHelper.GetAttribute<BuilderAttribute>(type);
        }
    }
}
