using Neptuo.Templates.Compilation.CodeObjects;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Base codeDom sub generator for code objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of code object.</typeparam>
    public abstract class BaseCodeDomObjectGenerator<T> : ICodeDomComponentGenerator
        where T : ICodeObject
    {
        public CodeExpression GenerateCode(CodeObjectExtensionContext context, ICodeObject codeObject, IPropertyDescriptor propertyDescriptor)
        {
            return GenerateCode(context, (T)codeObject, propertyDescriptor);
        }

        protected abstract CodeExpression GenerateCode(CodeObjectExtensionContext context, T codeObject, IPropertyDescriptor propertyDescriptor);
    }
}
