using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Token type for <see cref="ComposableToken"/>.
    /// </summary>
    public class TokenType
    {
        /// <summary>
        /// Plain text token type.
        /// </summary>
        public static readonly TokenType Literal = new TokenType("Literal");

        /// <summary>
        /// Whitespace only text token type.
        /// </summary>
        public static readonly TokenType Whitespace = new TokenType("Whitespace");

        /// <summary>
        /// Unique token type name.
        /// </summary>
        public string UniqueName { get; private set; }

        /// <summary>
        /// Creates instance of token type.
        /// </summary>
        /// <param name="uniqueName">Unique token type name.</param>
        public TokenType(string uniqueName)
        {
            Ensure.NotNullOrEmpty(uniqueName, "uniqueName");
            UniqueName = uniqueName;
        }


        public override int GetHashCode()
        {
            return 5 ^ UniqueName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TokenType otherType = obj as TokenType;
            if(otherType == null)
                return false;

            return UniqueName.Equals(otherType.UniqueName);
        }

        public override string ToString()
        {
            return String.Format("({0})", UniqueName);
        }
    }
}
