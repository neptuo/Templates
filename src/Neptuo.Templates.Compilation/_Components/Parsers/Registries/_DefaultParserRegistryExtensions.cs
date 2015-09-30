using Neptuo.Templates.Compilation.AssemblyScanning;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Common extensions for <see cref="DefaultParserRegistry"/> and <see cref="IParserProvider"/>.
    /// </summary>
    public static class _DefaultParserRegistryExtensions
    {
        #region TypeScanner

        /// <summary>
        /// Registers <paramref name="typeScanner"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="typeScanner">Instance of type scanner.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddTypeScanner(this DefaultParserRegistry registry, TypeScanner typeScanner)
        {
            Ensure.NotNull(registry, "registry");
            return registry.AddRegistry<TypeScanner>(typeScanner);
        }

        /// <summary>
        /// Returns registered instance of type scanner in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read type scanner from.</param>
        /// <returns>Registered instance of type scanner in <paramref name="registry"/>.</returns>
        public static TypeScanner WithTypeScanner(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.With<TypeScanner>();
        }

        /// <summary>
        /// Executes type scannes method <see cref="TypeScanner.Run"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read type scanner from.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry RunTypeScanner(this DefaultParserRegistry registry)
        {
            Ensure.NotNull(registry, "registry");
            registry.WithTypeScanner().Run();
            return registry;
        }

        #endregion

        #region IEnumerable<NamespaceDeclaration>

        /// <summary>
        /// Enumerates all registered prefixes and namespaces.
        /// </summary>
        /// <param name="registry">Parser registry to read registrations from.</param>
        /// <returns>Enumeration of all registered prefixes and namespaces.</returns>
        public static IEnumerable<NamespaceDeclaration> WithUsedNamespaces(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.WithTypeScanner().EnumerateUsedNamespaces();
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="registry"/> has registered enumeration of used prefixes and namespaces (<see cref="WithUsedNamespaces"/>).
        /// </summary>
        /// <param name="registry">Parser registry to read registrations from.</param>
        /// <returns><c>true</c> if <paramref name="registry"/> has registered enumeration of used prefixes and namespaces; otherwise <c>false</c>.</returns>
        public static bool HasUsedNamespaces(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.Has<TypeScanner>();
        }

        #endregion

        #region IContentBuilder

        /// <summary>
        /// Registers <paramref name="builder"/> to the <paramref name="registry"/> 
        /// and if <paramref name="registry"/> contains <see cref="TypeScanner"/>, attaches to mapping types (using <see cref="DefaultTypeComponentBuilder"/>).
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Content builder registry to regiter to <paramref name="registry"/> and (eventualy) attach to type scanner.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddContentBuilderRegistry(this DefaultParserRegistry registry, ContentBuilderRegistry builder)
        {
            Ensure.NotNull(registry, "registry");

            if (registry.Has<TypeScanner>())
            {
                TypeScanner typeScanner = registry.With<TypeScanner>();
                typeScanner.AddTypeProcessor((prefix, type) => builder.AddBuilder(
                    prefix, 
                    type.Name, 
                    new DefaultTypeComponentBuilder(type)
                ));
            }

            return registry.AddRegistry<IContentBuilder>(builder);
        }

        /// <summary>
        /// Registers <paramref name="builder"/> to the <paramref name="registry"/> 
        /// and if <paramref name="registry"/> contains <see cref="TypeScanner"/>, attaches to mapping types (using <paramref name="builderFactory" />).
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Content builder registry to regiter to <paramref name="registry"/> and (eventualy) attach to type scanner.</param>
        /// <param name="builderFactory">Factory method for registering content builders when type scanner emits type.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddContentBuilderRegistry(this DefaultParserRegistry registry, ContentBuilderRegistry builder, Func<string, Type, IContentBuilder> builderFactory)
        {
            Ensure.NotNull(registry, "registry");

            if (registry.Has<TypeScanner>())
            {
                TypeScanner typeScanner = registry.With<TypeScanner>();
                typeScanner.AddTypeProcessor((prefix, type) => builder.AddBuilder(
                    prefix,
                    type.Name,
                    builderFactory(prefix, type)
                ));
            }

            return registry.AddRegistry<IContentBuilder>(builder);
        }

        /// <summary>
        /// Register <paramref name="builder"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Instance of content builder.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddContentBuilder(this DefaultParserRegistry registry, IContentBuilder builder)
        {
            Ensure.NotNull(registry, "registry");
            return registry.AddRegistry<IContentBuilder>(builder);
        }

        /// <summary>
        /// Returns registered instance of content builder in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read content builder from.</param>
        /// <returns>Registered instance of content builder in <paramref name="registry"/>.</returns>
        public static IContentBuilder WithContentBuilder(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.With<IContentBuilder>();
        }

        #endregion

        #region ITokenBuilder

        /// <summary>
        /// Registers <paramref name="builder"/> to <paramref name="registry"/> and if <paramref name="registry"/> contains <see cref="TypeScanner"/>,
        /// attaches to mapping types.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Token builder registry to regiter to <paramref name="registry"/> and (eventualy) attach to type scanner.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddTokenBuilder(this DefaultParserRegistry registry, TokenBuilderRegistry builder)
        {
            Ensure.NotNull(registry, "registry");

            if (registry.Has<TypeScanner>())
            {
                TypeScanner typeScanner = registry.With<TypeScanner>();
                typeScanner.AddTypeProcessor((prefix, type) => builder.AddBuilder(
                    prefix, 
                    type.Name, 
                    new DefaultTypeTokenBuilder(type)
                ));
            }

            return registry.AddRegistry<ITokenBuilder>(builder);
        }

        /// <summary>
        /// Register <paramref name="builder"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Instance of token builder.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddTokenBuilder(this DefaultParserRegistry registry, ITokenBuilder builder)
        {
            Ensure.NotNull(registry, "registry");
            return registry.AddRegistry<ITokenBuilder>(builder);
        }

        /// <summary>
        /// Returns registered instance of token builder in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read token builder from.</param>
        /// <returns>Registered instance of token builder in <paramref name="registry"/>.</returns>
        public static ITokenBuilder WithTokenBuilder(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.With<ITokenBuilder>();
        }

        #endregion

        #region IContentPropertyBuilder

        /// <summary>
        /// Register <paramref name="builder"/> to the <paramref name="registry"/> (also under <see cref="IPropertyBuilder"/>).
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Instance of property builder.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddPropertyBuilder(this DefaultParserRegistry registry, IContentPropertyBuilder builder)
        {
            Ensure.NotNull(registry, "registry");
            return registry
                .AddRegistry<IContentPropertyBuilder>(builder)
                .AddRegistry<IPropertyBuilder>(builder);
        }

        /// <summary>
        /// Returns registered instance of content property builder in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read content property builder from.</param>
        /// <returns>Registered instance of content property builder in <paramref name="registry"/>.</returns>
        public static IContentPropertyBuilder WithContentPropertyBuilder(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.With<IContentPropertyBuilder>();
        }

        /// <summary>
        /// Returns registered instance of property builder in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read property builder from.</param>
        /// <returns>Registered instance of property builder in <paramref name="registry"/>.</returns>
        public static IPropertyBuilder XWithPropertyBuilder(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.With<IContentPropertyBuilder>();
        }

        #endregion

        #region ILiteralBuilder

        /// <summary>
        /// Register <paramref name="builder"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Instance of literal builder.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddLiteralBuilder(this DefaultParserRegistry registry, ILiteralBuilder builder)
        {
            Ensure.NotNull(registry, "registry");
            return registry.AddRegistry<ILiteralBuilder>(builder);
        }

        /// <summary>
        /// Returns registered instance of literal builder in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read literal builder from.</param>
        /// <returns>Registered instance of literal builder in <paramref name="registry"/>.</returns>
        public static ILiteralBuilder WithLiteralBuilder(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.With<ILiteralBuilder>();
        }

        #endregion

        #region INameNormalizer

        /// <summary>
        /// Register <paramref name="nameNormalizer"/> (with name 'Property') to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="nameNormalizer">Instance of name normalizer for properties.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry{T}(string, T)"/>.</returns>
        public static DefaultParserRegistry AddPropertyNormalizer(this DefaultParserRegistry registry, INameNormalizer nameNormalizer)
        {
            return registry.AddRegistry<INameNormalizer>("Property", nameNormalizer);
        }

        /// <summary>
        /// Returns registered instance of name normalizer (with name 'Property') in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read name normalizer from.</param>
        /// <returns>Registered instance of name normalizer (with name 'Property') in <paramref name="registry"/>.</returns>
        public static INameNormalizer WithPropertyNormalizer(this IParserProvider registry)
        {
            return registry.With<INameNormalizer>("Property");
        }

        #endregion
    }
}
