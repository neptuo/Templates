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
