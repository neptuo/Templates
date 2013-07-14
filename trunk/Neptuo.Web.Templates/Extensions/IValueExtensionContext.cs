using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Extensions
{
    public interface IValueExtensionContext
    {
        object TargetObject { get; }
        PropertyInfo TargetProperty { get; }

        //TODO: Really need this?
        IDependencyProvider DependencyProvider { get; }
    }
}