using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public abstract class BasePropertyDescriptorExtension<T> : IPropertyDescriptorExtension
        where T : IPropertyDescriptor
    {
        public void GenerateProperty(PropertyDescriptorExtensionContext context, IPropertyDescriptor propertyDescriptor)
        {
            GenerateProperty(context, (T)propertyDescriptor);
        }

        protected abstract void GenerateProperty(PropertyDescriptorExtensionContext context, T propertyDescriptor);
    }
}
