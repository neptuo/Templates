using Neptuo.Templates.Compilation.AssemblyScanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _DefaultParserRegistryExtensions
    {
        #region IContentBuilder

        /// <summary>
        /// Registers <paramref name="builder"/> to <paramref name="registry"/> and if <paramref name="registry"/> contains <see cref="TypeScanner"/>,
        /// attaches to mapping types.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Content builder registry to regiter to <paramref name="registry"/> and (eventualy) attach to type scanner.</param>
        /// <returns>Result from <see cref="DefaultParserRegistry.AddRegistry"/>.</returns>
        public static DefaultParserRegistry AddContentBuilderRegistry(this DefaultParserRegistry registry, ContentBuilderRegistry builder)
        {
            Guard.NotNull(registry, "registry");

            if (registry.Has<TypeScanner>())
            {
                TypeScanner typeScanner = registry.With<TypeScanner>();
                typeScanner.AddTypeProcessor((prefix, type) => builder.AddBuilder(
                    prefix, 
                    type.Name, 
                    new DefaultTypeComponentBuilder(
                        type, 
                        registry.WithContentPropertyBuilder(), 
                        registry.WithObserverBuilder()
                    )
                ));
            }

            return registry.AddRegistry<IContentBuilder>(builder);
        }

        public static DefaultParserRegistry AddContentBuilder(this DefaultParserRegistry registry, IContentBuilder builder)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<IContentBuilder>(builder);
        }

        public static IContentBuilder WithContentBuilder(this IParserRegistry registry)
        {
            Guard.NotNull(registry, "registry");
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
            Guard.NotNull(registry, "registry");

            if (registry.Has<TypeScanner>())
            {
                TypeScanner typeScanner = registry.With<TypeScanner>();
                typeScanner.AddTypeProcessor((prefix, type) => builder.AddBuilder(
                    prefix, 
                    type.Name, 
                    new DefaultTokenBuilder(
                        type, 
                        registry.WithContentPropertyBuilder()
                    )
                ));
            }

            return registry.AddRegistry<ITokenBuilder>(builder);
        }

        public static DefaultParserRegistry AddTokenBuilder(this DefaultParserRegistry registry, ITokenBuilder builder)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<ITokenBuilder>(builder);
        }

        public static ITokenBuilder WithTokenBuilder(this IParserRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<ITokenBuilder>();
        }

        #endregion

        #region IObserverBuilder

        public static DefaultParserRegistry AddObserverBuilder(this DefaultParserRegistry registry, IObserverBuilder builder)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<IObserverBuilder>(builder);
        }

        public static IObserverBuilder WithObserverBuilder(this IParserRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<IObserverBuilder>();
        }

        #endregion

        #region IContentPropertyBuilder

        public static DefaultParserRegistry AddContentPropertyBuilder(this DefaultParserRegistry registry, IContentPropertyBuilder builder)
        {
            Guard.NotNull(registry, "registry");
            return registry.AddRegistry<IContentPropertyBuilder>(builder);
        }

        public static IContentPropertyBuilder WithContentPropertyBuilder(this IParserRegistry registry)
        {
            Guard.NotNull(registry, "registry");
            return registry.With<IContentPropertyBuilder>();
        }

        #endregion
    }
}
