using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    /// <summary>
    /// Collection of supported comsable tokens.
    /// </summary>
    public class TokenTypeCollection : IEnumerable<TokenType>
    {
        private readonly HashSet<TokenType> storage;

        /// <summary>
        /// Creates new empty collection.
        /// </summary>
        public TokenTypeCollection()
            : this(Enumerable.Empty<TokenType>())
        { }

        /// <summary>
        /// Creates collection with initial token types.
        /// </summary>
        /// <param name="initialTokens">Initial token types.</param>
        public TokenTypeCollection(IEnumerable<TokenType> initialTokens)
        {
            storage = new HashSet<TokenType>(initialTokens);
        }

        /// <summary>
        /// Adds <paramref name="tokenType"/> to the collection.
        /// If <paramref name="tokenType"/> is <c>null</c>, throw <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="tokenType">Token type to add.</param>
        /// <returns>Self (for fluency).</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="tokenType"/> is <c>null</c>.</exception>
        public TokenTypeCollection Add(TokenType tokenType)
        {
            Ensure.NotNull(tokenType, "token");
            storage.Add(tokenType);
            return this;
        }

        /// <summary>
        /// Returns <c>true</c> if collection contains <paramref name="tokenType"/>; otherwise <c>false</c>.
        /// If <paramref name="tokenType"/> is <c>null</c>, throw <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="tokenType">Token to test.</param>
        /// <returns><c>true</c> if collection contains <paramref name="tokenType"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="tokenType"/> is <c>null</c>.</exception>
        public bool IsContained(TokenType tokenType)
        {
            Ensure.NotNull(tokenType, "token");
            return storage.Contains(tokenType);
        }

        #region IEnumerable

        public IEnumerator<TokenType> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
