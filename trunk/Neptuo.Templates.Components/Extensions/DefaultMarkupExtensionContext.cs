using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Extensions
{
    public class DefaultMarkupExtensionContext : IValueExtensionContext
    {
        public object TargetObject { get; set; }
        public PropertyInfo TargetProperty { get; set; }
        public IDependencyProvider DependencyProvider { get; set; }

        public DefaultMarkupExtensionContext(object targetObject, PropertyInfo targetProperty, IDependencyProvider dependencyProvider)
        {
            TargetObject = targetObject;
            TargetProperty = targetProperty;
            DependencyProvider = dependencyProvider;
        }
    }
}
