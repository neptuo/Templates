﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Collection of supported comsable tokens.
    /// </summary>
    public class ComposableTokenTypeCollection : IEnumerable<ComposableTokenType>
    {
        private readonly HashSet<ComposableTokenType> storage;

        /// <summary>
        /// Creates new empty collection.
        /// </summary>
        public ComposableTokenTypeCollection()
            : this(Enumerable.Empty<ComposableTokenType>())
        { }

        /// <summary>
        /// Creates collection with initial token types.
        /// </summary>
        /// <param name="initialTokens">Initial token types.</param>
        public ComposableTokenTypeCollection(IEnumerable<ComposableTokenType> initialTokens)
        {
            storage = new HashSet<ComposableTokenType>(initialTokens);
        }

        /// <summary>
        /// Adds <paramref name="tokenType"/> to the collection.
        /// If <paramref name="tokenType"/> is <c>null</c>, throw <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="tokenType">Token type to add.</param>
        /// <returns>Self (for fluency).</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="tokenType"/> is <c>null</c>.</exception>
        public ComposableTokenTypeCollection Add(ComposableTokenType tokenType)
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
        public bool IsContained(ComposableTokenType tokenType)
        {
            Ensure.NotNull(tokenType, "token");
            return storage.Contains(tokenType);
        }

        #region IEnumerable

        public IEnumerator<ComposableTokenType> GetEnumerator()
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