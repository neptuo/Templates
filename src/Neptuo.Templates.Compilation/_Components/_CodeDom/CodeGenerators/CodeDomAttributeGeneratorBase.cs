using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    /// <summary>
    /// Base implementation of <see cref="ICodeDomAttributeGenerator"/> which casts input attribute to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of attribute that is supported.</typeparam>
    public abstract class CodeDomAttributeGeneratorBase<T> : ICodeDomAttributeGenerator
        where T : Attribute
    {
        public CodeExpression GenerateCode(CodeDomAttributeContext context, Attribute attribute)
        {
            return GenerateCode(context, (T)attribute);
        }

        protected abstract CodeExpression GenerateCode(CodeDomAttributeContext context, T attribute);
    }
}
