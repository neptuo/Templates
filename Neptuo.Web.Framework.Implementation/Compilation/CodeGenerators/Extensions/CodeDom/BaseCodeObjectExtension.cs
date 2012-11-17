using Neptuo.Web.Framework.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Compilation.CodeGenerators.Extensions.CodeDom
{
    public abstract class BaseCodeObjectExtension<T> : ICodeObjectExtension
        where T : ICodeObject
    {
        public CodeExpression GenerateCode(CodeObjectExtensionContext context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return GenerateCode(context, (T)codeObject, propertyDescriptor);
        }

        protected abstract CodeExpression GenerateCode(CodeObjectExtensionContext context, T codeObject, IPropertyDescriptor propertyDescriptor);
    }
}
