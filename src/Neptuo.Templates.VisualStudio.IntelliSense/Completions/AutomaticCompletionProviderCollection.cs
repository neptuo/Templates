using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Implementation of <see cref="IAutomaticCompletionProvider"/> that provides automatic completions from multiple providers.
    /// </summary>
    public class AutomaticCompletionProviderCollection : IAutomaticCompletionProvider
    {
        private readonly List<IAutomaticCompletionProvider> providers = new List<IAutomaticCompletionProvider>();

        /// <summary>
        /// Adds <paramref name="provider"/> at the end of the collection.
        /// </summary>
        /// <param name="provider">The provider to add to the collection.</param>
        /// <returns>Self (for fluency).</returns>
        public AutomaticCompletionProviderCollection Add(IAutomaticCompletionProvider provider)
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
        public AutomaticCompletionProviderCollection Add(int index, IAutomaticCompletionProvider provider)
        {
            Ensure.PositiveOrZero(index, "index");
            Ensure.NotNull(provider, "provider");

            if (index > providers.Count)
                providers.Add(provider);
            else
                providers[index] = provider;

            return this;
        }

        public bool TryGet(TokenCursor currentToken, RelativePosition cursorPosition, out IAutomaticCompletion completion)
        {
            foreach (IAutomaticCompletionProvider provider in providers)
            {
                if (provider.TryGet(currentToken, cursorPosition, out completion))
                    return true;
            }

            completion = null;
            return false;
        }
    }
}
