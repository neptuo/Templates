using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Templates.Compilation.Parsers
{
    public partial class BaseParser
    {
        protected List<IDefaultValueExtension> DefaultValueExtensions { get; private set; }

        public BaseParser()
        {
            DefaultValueExtensions = new List<IDefaultValueExtension>();
            DefaultValueExtensions.Add(new DefaultAttributeExtension());
        }

        public bool BindPropertyDefaultValue(IPropertyDescriptor propertyDescriptor)
        {
            foreach (IDefaultValueExtension extension in DefaultValueExtensions)
            {
                if (extension.ProvideValue(propertyDescriptor))
                {
                    if (propertyDescriptor is IDefaultPropertyValue)
                        ((IDefaultPropertyValue)propertyDescriptor).IsDefaultValue = true;

                    return true;
                }
            }
            return false;
        }
    }
}
