using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultTokenBuilder : TokenDescriptorBuilder
    {
        protected Type Type { get; private set; }

        public DefaultTokenBuilder(Type type, IPropertyBuilder propertyFactory)
            : base(propertyFactory)
        {
            Guard.NotNull(type, "type");
            Type = type;
        }

        protected override IComponentCodeObject CreateCodeObject(ITokenBuilderContext context, Token extension)
        {
            return new ComponentCodeObject(Type);
        }

        protected override IComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, IComponentCodeObject codeObject, Token extension)
        {
            return new TypeComponentDescriptor(Type);
        }
    }
}
