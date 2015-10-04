using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public static class _DefaultParserRegistryExtensions
    {
        #region IObserverBuilder

        /// <summary>
        /// Register <paramref name="builder"/> to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="builder">Instance of observer builder.</param>
        /// <returns>Result from <see cref="DefaultParserCollection.AddRegistry"/>.</returns>
        public static DefaultParserCollection AddObserverBuilder(this DefaultParserCollection registry, IObserverBuilder builder)
        {
            Ensure.NotNull(registry, "registry");
            return registry.AddRegistry<IObserverBuilder>(builder);
        }

        /// <summary>
        /// Returns registered instance of observer builder in <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to read observer builder from.</param>
        /// <returns>Registered instance of observer builder in <paramref name="registry"/>.</returns>
        public static IObserverBuilder WithObserverBuilder(this IParserProvider registry)
        {
            Ensure.NotNull(registry, "registry");
            return registry.With<IObserverBuilder>();
        }

        #endregion

    }
}
