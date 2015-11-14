using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense.Completions
{
    /// <summary>
    /// Implementation of <see cref="ICompletionProvider"/> that provides completions from multiple providers.
    /// </summary>
    public class CompletionProviderCollection : ICompletionProvider
    {
        private readonly List<ICompletionProvider> providers = new List<ICompletionProvider>();

        /// <summary>
        /// Adds <paramref name="provider"/> at the end of the collection.
        /// </summary>
        /// <param name="provider">The provider to add to the collection.</param>
        /// <returns>Self (for fluency).</returns>
        public CompletionProviderCollection Add(ICompletionProvider provider)
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
        public CompletionProviderCollection Add(int index, ICompletionProvider provider)
        {
            Ensure.PositiveOrZero(index, "index");
            Ensure.NotNull(provider, "provider");

            if (index > providers.Count)
                providers.Add(provider);
            else
                providers[index] = provider;

            return this;
        }

        public IEnumerable<ICompletion> GetCompletions(IReadOnlyList<Token> tokens, Token currentToken)
        {
            return providers.SelectMany(p => p.GetCompletions(tokens, currentToken));
        }
    }
}
