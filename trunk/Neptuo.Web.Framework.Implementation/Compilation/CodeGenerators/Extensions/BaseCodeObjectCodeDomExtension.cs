using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions
{
    public abstract class BaseCodeDomCodeObjectExtension<T> : ICodeDomCodeObjectExtension
        where T : ICodeObject
    {
        public CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return GenerateCode(context, (T)codeObject, propertyDescriptor);
        }

        protected abstract CodeExpression GenerateCode(CodeDomCodeObjectExtensionContext context, T codeObject, IPropertyDescriptor propertyDescriptor);
    }
}
