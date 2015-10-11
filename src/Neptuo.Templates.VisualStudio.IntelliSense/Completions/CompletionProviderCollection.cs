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
        private readonly List<ICompletionProvider> storage = new List<ICompletionProvider>();

        /// <summary>
        /// Adds <paramref name="provider"/> to collection of providers used to provide completions.
        /// </summary>
        /// <param name="provider">The provider to add to the collection.</param>
        /// <returns>Self (for fluency).</returns>
        public CompletionProviderCollection Add(ICompletionProvider provider)
        {
            Ensure.NotNull(provider, "provider");
            storage.Add(provider);
            return this;
        }

        public IEnumerable<ICompletion> GetCompletions(IReadOnlyList<Token> tokens, Token currentToken)
        {
            return storage.SelectMany(p => p.GetCompletions(tokens, currentToken));
        }
    }
}
