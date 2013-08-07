using Neptuo.Templates.Compilation.CodeObjects;
using Neptuo.Templates.Compilation.Parsers.ExtensionContent;
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

        protected override IValueExtensionCodeObject CreateCodeObject(IMarkupExtensionBuilderContext context, Extension extension)
        {
            return new ExtensionCodeObject(Type);
        }

        protected override IExtensionDefinition GetExtensionDefinition(IMarkupExtensionBuilderContext context, IValueExtensionCodeObject codeObject, Extension extension)
        {
            return new TypeDefinition(Type);
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
