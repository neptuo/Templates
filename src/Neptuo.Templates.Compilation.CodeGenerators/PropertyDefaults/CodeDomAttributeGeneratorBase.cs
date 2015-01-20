using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.CodeGenerators
{
    public abstract class CodeDomAttributeGeneratorBase<T> : ICodeDomAttributeGenerator
        where T : Attribute
    {
        public ICodeDomAttributeResult Generate(ICodeDomContext context, Attribute attribute)
        {
            return Generate(context, attribute);
        }

        protected abstract ICodeDomAttributeResult Generate(ICodeDomContext context, T attribute);
    }
}
