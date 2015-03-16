using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
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
            return new ComponentCodeObject(Type);
        }

        protected override IComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, ICodeObject codeObject, Token extension)
        {
            return new TypeComponentDescriptor(Type);
        }
    }
}
