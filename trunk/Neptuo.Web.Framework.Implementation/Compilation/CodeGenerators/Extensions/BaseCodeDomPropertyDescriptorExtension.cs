using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public abstract class BaseCodeDomPropertyDescriptorExtension<T> : ICodeDomPropertyDescriptorExtension
        where T : IPropertyDescriptor
    {
        public void GenerateProperty(CodeDomPropertyDescriptorExtensionContext context, IPropertyDescriptor propertyDescriptor)
        {
            GenerateProperty(context, (T)propertyDescriptor);
        }

        protected abstract void GenerateProperty(CodeDomPropertyDescriptorExtensionContext context, T propertyDescriptor);
    }
}
