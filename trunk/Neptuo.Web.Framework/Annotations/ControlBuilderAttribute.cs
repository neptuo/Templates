using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Utils;

namespace Neptuo.Web.Framework.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControlBuilderAttribute : Attribute
    {
        public Type BuilderType { get; protected set; }

        public ControlBuilderAttribute(Type builderType)
        {
            BuilderType = builderType;
        }

        public static ControlBuilderAttribute GetAttribute(Type type)
        {
            return ReflectionHelper.GetAttribute<ControlBuilderAttribute>(type);
        }
    }
}
