using Neptuo.Web.Framework.Compilation.CodeObjects;
using Neptuo.Web.Framework.Compilation.Parsers.Extensions;
using Neptuo.Web.Framework.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.Parsers
{
    public partial class BaseParser
    {
        protected List<IDefaultValueExtension> DefaultValueExtensions {get; private set;}

        public BaseParser()
        {
            DefaultValueExtensions = new List<IDefaultValueExtension>();
            DefaultValueExtensions.Add(new ComponentManagerExtension());
            DefaultValueExtensions.Add(new DefaultAttributeExtension());
        }

        protected bool BindPropertyDefaultValue(IPropertyDescriptor propertyDescriptor)
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
