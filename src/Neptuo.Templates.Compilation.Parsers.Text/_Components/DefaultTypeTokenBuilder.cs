using Neptuo.Models.Features;
using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.CodeObjects.Features;
using Neptuo.Templates.Compilation.Parsers.Descriptors;
using Neptuo.Templates.Compilation.Parsers.Descriptors.Features;
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

        protected override IComponentCodeObject CreateCodeObject(ITokenBuilderContext context, Token extension)
        {
            ComponentCodeObject codeObject = new ComponentCodeObject();
            codeObject
                .Add<ITypeCodeObject>(new TypeCodeObject(Type))
                .Add<IFieldCollectionCodeObject>(new FieldCollectionCodeObject());

            return codeObject;
        }

        protected override IComponentDescriptor GetComponentDescriptor(ITokenBuilderContext context, ICodeObject codeObject, Token extension)
        {
            DefaultComponentDescriptor component = new DefaultComponentDescriptor();
            component
                .Add<IFieldEnumerator>(new TypePropertyFieldEnumerator(Type))
                .Add<IDefaultFieldEnumerator>(new DefaultPropertyFieldEnumerator(Type));

            return component;
        }
    }
}
