using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Test.Templates
{
    /// <summary>
    /// Default implementation of <see cref="IValueExtensionContext"/>.
    /// </summary>
    public class DefaultValueExtensionContext : IValueExtensionContext
    {
        public object TargetObject { get; set; }
        public PropertyInfo TargetProperty { get; set; }
        public IDependencyProvider DependencyProvider { get; set; }

        public DefaultValueExtensionContext(object targetObject, PropertyInfo targetProperty, IDependencyProvider dependencyProvider)
        {
            Guard.NotNull(targetObject, "targetObject");
            Guard.NotNull(targetProperty, "targetProperty");
            Guard.NotNull(dependencyProvider, "dependencyProvider");
            TargetObject = targetObject;
            TargetProperty = targetProperty;
            DependencyProvider = dependencyProvider;
        }
    }
}
