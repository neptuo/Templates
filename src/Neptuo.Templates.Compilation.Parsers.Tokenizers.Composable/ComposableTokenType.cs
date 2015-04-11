using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Token type for <see cref="ComposableToken"/>.
    /// </summary>
    public class ComposableTokenType
    {
        /// <summary>
        /// Error token type.
        /// </summary>
        public static readonly ComposableTokenType Error = new ComposableTokenType("Error");

        /// <summary>
        /// Plain text token type.
        /// </summary>
        public static readonly ComposableTokenType Text = new ComposableTokenType("Text");


        /// <summary>
        /// Unique token type name.
        /// </summary>
        public string UniqueName { get; private set; }

        /// <summary>
        /// Creates instance of token type.
        /// </summary>
        /// <param name="uniqueName">Unique token type name.</param>
        public ComposableTokenType(string uniqueName)
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
            ComposableTokenType otherType = obj as ComposableTokenType;
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
