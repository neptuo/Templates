using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class DefaultMarkupExtensionBuilder : BaseMarkupExtensionBuilder
    {
        protected Type Type { get; private set; }

        public DefaultMarkupExtensionBuilder(Type type)
        {
            Type = type;
        }

        protected override IValueExtensionCodeObject CreateCodeObject(IMarkupExtensionBuilderContext context, Token extension)
        {
            return new ExtensionCodeObject(Type);
        }

        protected override IMarkupExtensionInfo GetExtensionDefinition(IMarkupExtensionBuilderContext context, IValueExtensionCodeObject codeObject, Token extension)
        {
            return new TypeInfo(Type);
        }

        protected override IPropertyDescriptor CreateSetPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new SetPropertyDescriptor(propertyInfo);
        }

        protected override IPropertyDescriptor CreateListAddPropertyDescriptor(IPropertyInfo propertyInfo)
        {
            return new ListAddPropertyDescriptor(propertyInfo);
        }
    }
}
