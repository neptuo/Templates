using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class TokenListReader
    {
        private readonly IList<ComposableToken> tokens;

        /// <summary>
        /// Current reader position/index.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Token at index <see cref="TokenListReader.Position"/>.
        /// </summary>
        public ComposableToken Current
        {
            get
            {
                if (Position >= 0 && Position <= tokens.Count)
                    return tokens[Position];

                return null;
            }
        }

        public TokenListReader(IList<ComposableToken> tokens, int startIndex = -1)
        {
            Ensure.NotNull(tokens, "tokens");
            this.tokens = tokens;
            Position = startIndex;
        }

        /// <summary>
        /// Tries to read next token.
        /// Returns <c>true</c> if reading was successfull; <c>false</c> when end of token list was reached.
        /// </summary>
        /// <returns><c>true</c> if reading was successfull; <c>false</c> when end of token list was reached.</returns>
        public bool Next()
        {
            if(Position < tokens.Count)
            {
                Position++;
                return Position < tokens.Count;
            }

            return false;
        }

        /// <summary>
        /// Tries to read previous token.
        /// </summary>
        /// Returns <c>true</c> if reading was successfull; <c>false</c> when start of token list was reached.
        /// </summary>
        /// <returns><c>true</c> if reading was successfull; <c>false</c> when start of token list was reached.</returns>
        public bool Prev()
        {
            if (Position >= 0)
            {
                Position--;
                return Position >= 0;
            }

            return false;
        }
    }
}
