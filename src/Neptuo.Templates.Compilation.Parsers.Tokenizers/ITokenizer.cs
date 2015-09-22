using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Describes component that passes throught reader and creates list of syntactic tokens.
    /// </summary>
    /// <typeparam name="TToken">Type of token.</typeparam>
    public interface ITokenizer<TToken>
        where TToken : IToken
    {
        /// <summary>
        /// Processes <paramref name="reader"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="reader">Input content reader.</param>
        /// <param name="context">Context for input processing.</param>
        /// <returns>List of syntactic tokens from <paramref name="reader"/>.</returns>
        IList<TToken> Tokenize(IContentReader reader, ITokenizerContext context);
    }
}
