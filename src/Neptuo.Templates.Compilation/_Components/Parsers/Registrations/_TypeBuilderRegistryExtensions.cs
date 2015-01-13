using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _TypeBuilderRegistryExtensions
    {
        public static TypeBuilderRegistry RegisterDefaultNamespace(this TypeBuilderRegistry builderRegistry, string clrNamespace)
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            return builderRegistry.RegisterNamespace(null, clrNamespace);
        }

        public static TypeBuilderRegistry RegisterRootBuilder(this TypeBuilderRegistry builderRegistry, IContentBuilder rootBuilder)
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            Guard.NotNull(rootBuilder, "rootBuilder");
            return builderRegistry.RegisterComponentBuilder(null, XmlContentParser.FakeRootElementName, rootBuilder);
        }

        public static TypeBuilderRegistry RegisterPropertyBuilder<TPropertyType, TPropertyBuilderType>(this TypeBuilderRegistry builderRegistry, TPropertyBuilderType propertyBuilder)
            where TPropertyBuilderType : IPropertyBuilder, IContentPropertyBuilder
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            builderRegistry.RegisterPropertyBuilder(typeof(TPropertyType), (IPropertyBuilder)propertyBuilder);
            return builderRegistry.RegisterPropertyBuilder(typeof(TPropertyType), (IContentPropertyBuilder)propertyBuilder);
        }
    }
}
