using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class TokenDefaultTypeValueBuilder : TokenDescriptorValueBuilder
    {
        protected Type Type { get; private set; }

        public TokenDefaultTypeValueBuilder(Type type)
        {
            Guard.NotNull(type, "type");
            Type = type;
        }

        protected override ICodeObject CreateCodeObject(ITokenValueBuilderContext context, Token extension)
        {
            return new ComponentCodeObject(Type);
        }

        protected override IComponentDescriptor GetComponentDescriptor(ITokenValueBuilderContext context, ICodeObject codeObject, Token extension)
        {
            return new TypeComponentDescriptor(Type);
        }
    }
}
