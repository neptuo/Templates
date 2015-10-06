using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    /// <summary>
    /// Common extensions for <see cref="TokenTypeCollection"/>.
    /// </summary>
    public static class _TokenTypeCollectionExtensions
    {
        /// <summary>
        /// Adds elements from <paramref name="tokenTypes"/> to <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="tokenTypes"></param>
        /// <returns></returns>
        public static TokenTypeCollection AddRange(this TokenTypeCollection collection, IEnumerable<TokenType> tokenTypes)
        {
            Ensure.NotNull(collection, "collection");
            Ensure.NotNull(tokenTypes, "tokenTypes");

            foreach (TokenType tokenType in tokenTypes)
                collection.Add(tokenType);

            return collection;
        }
    }
}
