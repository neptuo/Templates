using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public interface ICodeDomCodeObjectExtension
    {
        CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor);
    }
}
