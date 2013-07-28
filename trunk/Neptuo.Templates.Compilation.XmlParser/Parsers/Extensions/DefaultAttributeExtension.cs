using Neptuo.Reflection;
using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers.Extensions
{
    public class DefaultAttributeExtension : IDefaultValueExtension
    {
        public bool ProvideValue(IPropertyDescriptor propertyDescriptor)
        {
            DefaultValueAttribute defaultValue = null;// ReflectionHelper.GetAttribute<DefaultValueAttribute>(propertyDescriptor.PropertyName);
            if (defaultValue != null)
            {
                propertyDescriptor.SetValue(new PlainValueCodeObject(defaultValue.Value));
                return true;
            }
            return false;
        }
    }
}
