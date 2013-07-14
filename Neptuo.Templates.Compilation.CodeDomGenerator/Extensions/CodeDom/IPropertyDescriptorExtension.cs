using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators.Extensions.CodeDom
{
    public interface IPropertyDescriptorExtension
    {
        void GenerateProperty(PropertyDescriptorExtensionContext context, IPropertyDescriptor propertyDescriptor);
    }
}
