using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers.Extensions
{
    public class ComponentManagerExtension : IDefaultValueExtension
    {
        public bool ProvideValue(IPropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Property.PropertyType == typeof(IComponentManager))
            {
                propertyDescriptor.SetValue(new DependencyCodeObject(typeof(IComponentManager)));
                return true;
            }
            return false;
        }
    }
}
