using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class TokenReader
    {
        public IList<Token> Tokens { get; private set; }

        public bool IsSkippedTokensSkipped { get; private set; }

        /// <summary>
        /// Current reader position/index.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Token at index <see cref="TokenReader.Position"/>.
        /// </summary>
        public Token Current
        {
            get
            {
                if (Position >= 0 && Position <= Tokens.Count)
                    return Tokens[Position];

                return null;
            }
        }

        public TokenReader(IList<Token> tokens, bool isSkippedTokensSkipped = true, int startIndex = -1)
        {
            Ensure.NotNull(tokens, "tokens");
            Tokens = tokens;
            IsSkippedTokensSkipped = isSkippedTokensSkipped;
            Position = startIndex;
        }

        /// <summary>
        /// Tries to read next token.
        /// Returns <c>true</c> if reading was successfull; <c>false</c> when end of token list was reached.
        /// </summary>
        /// <returns><c>true</c> if reading was successfull; <c>false</c> when end of token list was reached.</returns>
        public bool Next()
        {
            if(Position < Tokens.Count)
            {
                Position++;
                return Position < Tokens.Count && (!IsSkippedTokensSkipped || !Current.IsSkipped || Next());
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
