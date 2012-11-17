using Neptuo.Web.Framework.Compilation.CodeGenerators;
using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers.Extensions
{
    public class LocalFieldExtension : IDefaultValueExtension
    {
        public bool ProvideValue(IPropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Property.PropertyType == typeof(IComponentManager))
            {
                propertyDescriptor.SetValue(new LocalFieldCodeObject(CodeDomGenerator.Names.ComponentManagerField));
                return true;
            }
            if (propertyDescriptor.Property.PropertyType == typeof(IViewPage))
            {
                propertyDescriptor.SetValue(new LocalFieldCodeObject(CodeDomGenerator.Names.ViewPageField));
                return true;
            }
            if (propertyDescriptor.Property.PropertyType == typeof(IDependencyProvider))
            {
                propertyDescriptor.SetValue(new LocalFieldCodeObject(CodeDomGenerator.Names.DependencyProviderField));
                return true;
            }
            return false;
        }
    }
}
