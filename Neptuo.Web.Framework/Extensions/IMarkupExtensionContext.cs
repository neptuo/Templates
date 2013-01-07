using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Extensions
{
    public interface IMarkupExtensionContext
    {
        object TargetObject { get; }
        PropertyInfo TargetProperty { get; }

        //TODO: Really need this?
        IDependencyProvider DependencyProvider { get; }
    }
}