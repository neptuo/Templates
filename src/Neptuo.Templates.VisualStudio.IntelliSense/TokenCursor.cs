using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.VisualStudio.IntelliSense
{
    /// <summary>
    /// Pointer to concrete token in array of tokens.
    /// </summary>
    public class TokenCursor
    {
        private readonly IReadOnlyList<Token> tokens;
        private readonly int index;

        /// <summary>
        /// Creates new instance of cursor pointing at <paramref name="index"/> in <paramref name="tokens"/>.
        /// </summary>
        /// <param name="tokens">The list of tokens to move on.</param>
        /// <param name="index">The index in <paramref name="tokens"/> to point at.</param>
        public TokenCursor(IReadOnlyList<Token> tokens, int index)
        {
            Ensure.NotNull(tokens, "tokens");
            Ensure.PositiveOrZero(index, "index");
            if(index >= tokens.Count)
                throw Ensure.Exception.ArgumentOutOfRange("index", "Index ('{2}') must be between 0 and size of the tokens list ('{1}').", tokens.Count, index);

            this.tokens = tokens;
            this.index = index;
        }

        /// <summary>
        /// Gets current token.
        /// </summary>
        public Token Current
        {
            get { return tokens[index]; }
        }

        /// <summary>
        /// Returns <c>true</c> if tokens array contains next item and next cursor can be created; <c>false</c> otherwise.
        /// </summary>
        /// <returns><c>true</c> if tokens array contains next item and next cursor can be created; <c>false</c> otherwise.</returns>
        public bool HasNext()
        {
            return index + 1 < tokens.Count;
        }

        /// <summary>
        /// Tries to create next cursor if possible.
        /// </summary>
        /// <param name="cursor">The cursor to the next token or <c>null</c>.</param>
        /// <returns><c>true</c> if tokens array contains next item and next cursor is set; <c>false</c> otherwise.</returns>
        public bool TryNext(out TokenCursor cursor)
        {
            if(HasNext())
            {
                cursor = new TokenCursor(tokens, index + 1);
                return true;
            }

            cursor = null;
            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if tokens array contains previous item and previous cursor can be created; <c>false</c> otherwise.
        /// </summary>
        /// <returns><c>true</c> if tokens array contains previous item and previous cursor can be created; <c>false</c> otherwise.</returns>
        public bool HasPrev()
        {
            return index - 1 >= 0;
        }

        /// <summary>
        /// Tries to create previous cursor if possible.
        /// </summary>
        /// <param name="cursor">The cursor to the previous token or <c>null</c>.</param>
        /// <returns><c>true</c> if tokens array contains previous item and previous cursor is set; <c>false</c> otherwise.</returns>
        public bool TryPrev(out TokenCursor cursor)
        {
            if (HasPrev())
            {
                cursor = new TokenCursor(tokens, index - 1);
                return true;
            }

            cursor = null;
            return false;
        }
    }
}
