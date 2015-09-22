using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Text.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTypeTokenBuilder : TokenDescriptorBuilder
    {
        protected Type Type { get; private set; }

        public DefaultTypeTokenBuilder(Type type)
        {
            Ensure.NotNull(type, "type");
            Type = type;
        }

        protected override ICodeObject CreateCodeObject(ITokenBuilderContext context, Token extension)
        {
            return new XComponentCodeObject(Type);
        }

        protected override IXComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, ICodeObject codeObject, Token extension)
        {
            return new TypeComponentDescriptor(Type);
        }
    }
}
