using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Collection of supported comsable tokens.
    /// </summary>
    public class ComposableTokenCollection
    {
        private readonly HashSet<ComposableToken> storage;

        /// <summary>
        /// Creates new empty collection.
        /// </summary>
        public ComposableTokenCollection()
            : this(Enumerable.Empty<ComposableToken>())
        { }

        /// <summary>
        /// Creates collection with initial tokens.
        /// </summary>
        /// <param name="initialTokens">Initial tokens.</param>
        public ComposableTokenCollection(IEnumerable<ComposableToken> initialTokens)
        {
            storage = new HashSet<ComposableToken>(initialTokens);
        }

        /// <summary>
        /// Adds <paramref name="token"/> to the collection.
        /// If <paramref name="token"/> is <c>null</c>, throw <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="token">Token to add.</param>
        /// <returns>Self (for fluency).</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="token"/> is <c>null</c>.</exception>
        public ComposableTokenCollection Add(ComposableToken token)
        {
            Ensure.NotNull(token, "token");
            storage.Add(token);
            return this;
        }

        /// <summary>
        /// Returns <c>true</c> if collection contains <paramref name="token"/>; otherwise <c>false</c>.
        /// If <paramref name="token"/> is <c>null</c>, throw <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="token">Token to test.</param>
        /// <returns><c>true</c> if collection contains <paramref name="token"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="token"/> is <c>null</c>.</exception>
        public bool IsContained(ComposableToken token)
        {
            Ensure.NotNull(token, "token");
            return storage.Contains(token);
        }
    }
}
