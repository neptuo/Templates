using Neptuo.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        /// <summary>
        /// Registers component builder for fake XML root provided by <see cref="XmlContentParser"/>.
        /// </summary>
        public static TypeBuilderRegistry RegisterRootBuilder(this TypeBuilderRegistry builderRegistry, IContentBuilder rootBuilder)
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            Guard.NotNull(rootBuilder, "rootBuilder");
            return builderRegistry.RegisterComponentBuilder(null, XmlContentParser.FakeRootElementName, rootBuilder);
        }

        /// <summary>
        /// Registers component builder for fake XML root provided by <see cref="XmlContentParser"/> with <see cref="RootContentBuilder"/>.
        /// </summary>
        public static TypeBuilderRegistry RegisterRootBuilder<T>(this TypeBuilderRegistry builderRegistry, Expression<Func<T, ICollection<object>>> rootPropertyGetter)
        {
            return RegisterRootBuilder(
                builderRegistry, 
                new RootContentBuilder(
                    builderRegistry, 
                    builderRegistry, 
                    new TypePropertyInfo(typeof(T).GetProperty(TypeHelper.PropertyName(rootPropertyGetter)))
                )
            );
        }

        /// <summary>
        /// Registers observer <typeparamref name="T"/> with <see cref="DefaultTypeObserverBuilder"/>
        /// </summary>
        public static TypeBuilderRegistry RegisterObserverBuilder<T>(this TypeBuilderRegistry builderRegistry, string prefix, string attributeName)
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            return builderRegistry.RegisterObserverBuilder(prefix, attributeName, new DefaultTypeObserverBuilder(typeof(T), builderRegistry));
        }

        public static TypeBuilderRegistry RegisterPropertyBuilder<TPropertyType, TPropertyBuilderType>(this TypeBuilderRegistry builderRegistry, TPropertyBuilderType propertyBuilder)
            where TPropertyBuilderType : IPropertyBuilder, IContentPropertyBuilder
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            builderRegistry.RegisterPropertyBuilder(typeof(TPropertyType), (IPropertyBuilder)propertyBuilder);
            return builderRegistry.RegisterPropertyBuilder(typeof(TPropertyType), (IContentPropertyBuilder)propertyBuilder);
        }

        public static TypeBuilderRegistry RegisterPropertyBuilder<TPropertyType, TPropertyBuilderType>(this TypeBuilderRegistry builderRegistry)
            where TPropertyBuilderType : IPropertyBuilder, new()
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            return builderRegistry.RegisterPropertyBuilder(typeof(TPropertyType), new TPropertyBuilderType());
        }

        public static TypeBuilderRegistry RegisterContentPropertyBuilder<TPropertyType, TPropertyBuilderType>(this TypeBuilderRegistry builderRegistry)
            where TPropertyBuilderType : IContentPropertyBuilder, new()
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            return builderRegistry.RegisterPropertyBuilder(typeof(TPropertyType), new TPropertyBuilderType());
        }

        /// <summary>
        /// Registers observer for HTML attributes, <see cref="HtmlAttributeObserverBuilder"/>.
        /// </summary>
        public static TypeBuilderRegistry RegisterHtmlAttributeObserverBuilder<T>(this TypeBuilderRegistry builderRegistry, Expression<Func<T, Dictionary<string, string>>> propertyGetter)
        {
            Guard.NotNull(builderRegistry, "builderRegistry");
            return builderRegistry.RegisterObserverBuilder(null, "*", new HtmlAttributeObserverBuilder(typeof(T), TypeHelper.PropertyName(propertyGetter)));
        }
    }
}
