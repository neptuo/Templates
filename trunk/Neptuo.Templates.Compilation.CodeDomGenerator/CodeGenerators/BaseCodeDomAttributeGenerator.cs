using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class BaseCodeDomAttributeGenerator<T> : ICodeDomAttributeGenerator
        where T : Attribute
    {
        public CodeExpression GenerateCode(CodeDomAttributeContext context, Attribute attribute)
        {
            return GenerateCode(context, (T)attribute);
        }

        protected abstract CodeExpression GenerateCode(CodeDomAttributeContext context, T attribute);
    }
}
