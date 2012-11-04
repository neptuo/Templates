using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers.Extensions
{
    public class DefaultAttributeExtension : IDefaultValueExtension
    {
        public bool ProvideValue(IPropertyDescriptor propertyDescriptor)
        {
            DefaultValueAttribute defaultValue = ReflectionHelper.GetAttribute<DefaultValueAttribute>(propertyDescriptor.Property);
            if (defaultValue != null)
            {
                propertyDescriptor.SetValue(new PlainValueCodeObject(defaultValue.Value));
                return true;
            }
            return false;
        }
    }
}
