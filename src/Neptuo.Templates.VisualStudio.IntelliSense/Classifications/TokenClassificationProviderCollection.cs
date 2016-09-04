using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Classifications
{
    /// <summary>
    /// Composite implementation of <see cref="ITokenClassificationProvider"/>.
    /// Calls registered providers until first one returns success.
    /// </summary>
    public class TokenClassificationProviderCollection : ITokenClassificationProvider
    {
        private readonly List<ITokenClassificationProvider> providers = new List<ITokenClassificationProvider>();

        /// <summary>
        /// Adds <paramref name="provider"/> at the end of collection.
        /// </summary>
        /// <param name="provider">Provider to add.</param>
        /// <returns>Self (for fluency).</returns>
        public TokenClassificationProviderCollection Add(ITokenClassificationProvider provider)
        {
            Ensure.NotNull(provider, "provider");
            providers.Add(provider);
            return this;
        }

        /// <summary>
        /// Adds <paramref name="provider"/> at <paramref name="index"/>.
        /// If <paramref name="index"/> if greater than size of collection <paramref name="provider"/> is added as last one.
        /// </summary>
        /// <param name="index">The index to insert at.</param>
        /// <param name="provider">The provider to add.</param>
        /// <returns>Self (for fluency).</returns>
        public TokenClassificationProviderCollection Add(int index, ITokenClassificationProvider provider)
        {
            Ensure.PositiveOrZero(index, "index");
            Ensure.NotNull(provider, "provider");
            
            if (index > providers.Count)
                providers.Add(provider);
            else
                providers[index] = provider;

            return this;
        }

        public bool TryGet(TokenType tokenType, out string classificationName)
        {
            foreach (ITokenClassificationProvider provider in providers)
            {
                if (provider.TryGet(tokenType, out classificationName))
                    return true;
            }

            classificationName = null;
            return false;
        }
    }
}
