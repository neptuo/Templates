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
    public interface IToken
    {
        /// <summary>
        /// Token line info.
        /// </summary>
        IDocumentSpan LineInfo { get; set; }

        /// <summary>
        /// Token content info.
        /// </summary>
        ITextSpan ContentInfo { get; set; }

        /// <summary>
        /// Token text.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Token error description.
        /// </summary>
        IList<IErrorMessage> Errors { get; }

        /// <summary>
        /// Whether this token is created without content in the source.
        /// </summary>
        bool IsVirtual { get; set; }

        /// <summary>
        /// Whether this token should be skipped.
        /// </summary>
        bool IsSkipped { get; set; }
    }

    /// <summary>
    /// Extends <see cref="IToken"/> by adding type of the token.
    /// </summary>
    /// <typeparam name="TType">Type of the token type.</typeparam>
    public interface IToken<TType> : IToken
    {
        /// <summary>
        /// Token type.
        /// </summary>
        TType Type { get; set; }
    }
}
