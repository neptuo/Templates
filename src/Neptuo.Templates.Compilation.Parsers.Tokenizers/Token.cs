using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using Neptuo.Text.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Describes base contract for <see cref="ITokenizer"/> output.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public TokenType Type { get; set; }

        /// <summary>
        /// Token line info.
        /// </summary>
        public IDocumentSpan DocumentSpan { get; set; }

        /// <summary>
        /// Token content info.
        /// </summary>
        public ITextSpan TextSpan { get; set; }

        /// <summary>
        /// Token text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Token error description.
        /// </summary>
        public IList<IErrorMessage> Errors { get; private set; }


        /// <summary>
        /// Whether this token is created without content in the source.
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Whether this token should be skipped.
        /// </summary>
        public bool IsSkipped { get; set; }

        /// <summary>
        /// Whether this token contains errors.
        /// </summary>
        public bool HasError
        {
            get { return Errors.Count > 0; }
        }

        public Token()
        { }

        public Token(TokenType type, string text)
        {
            Ensure.NotNull(type, "type");
            Type = type;
            Text = text;
            Errors = new List<IErrorMessage>();
        }

        public override string ToString()
        {
            string position = null;
            if (IsVirtual)
                position = "<Virtual>";
            else if (TextSpan != null)
                position = TextSpan.ToString();
            else if (DocumentSpan != null)
                position = DocumentSpan.ToString();

            return String.Format("{0} {1} \"{2}\"{3}", position, Type, Text, HasError ? " !" : "");
        }
    }
}
