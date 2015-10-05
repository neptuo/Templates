using Neptuo.Templates.Compilation.AssemblyScanning;
using Neptuo.Templates.Compilation.Parsers.Normalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    /// <summary>
    /// Common extensions for <see cref="DefaultParserCollection"/> and <see cref="IParserProvider"/>.
    /// </summary>
    public static class _DefaultParserProviderExtensions
    {
        #region INameNormalizer

        /// <summary>
        /// Register <paramref name="nameNormalizer"/> (with name 'Property') to the <paramref name="registry"/>.
        /// </summary>
        /// <param name="registry">Parser registry to register to.</param>
        /// <param name="nameNormalizer">Instance of name normalizer for properties.</param>
        /// <returns>Result from <see cref="DefaultParserCollection.Add{T}(string, T)"/>.</returns>
        public static DefaultParserCollection AddPropertyNormalizer(this DefaultParserCollection registry, INameNormalizer nameNormalizer)
        {
            return registry.Add<INameNormalizer>("Property", nameNormalizer);
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
