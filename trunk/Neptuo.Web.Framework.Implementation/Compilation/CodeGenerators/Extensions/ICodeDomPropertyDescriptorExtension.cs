using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public interface ICodeDomPropertyDescriptorExtension
    {
        void GenerateProperty(CodeDomPropertyDescriptorExtensionContext context, IPropertyDescriptor propertyDescriptor);
    }
}
