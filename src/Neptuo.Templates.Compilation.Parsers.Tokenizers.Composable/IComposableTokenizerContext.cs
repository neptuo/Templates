using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Context for <see cref="IComposableTokenizer"/>.
    /// </summary>
    public interface IComposableTokenizerContext : ITokenizerContext
    {
        /// <summary>
        /// Processes <paramref name="reader"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="reader">Input content reader.</param>
        /// <param name="initiator">Tokenizer, that should be skipped.</param>
        /// <returns>List of syntactic tokens from <paramref name="reader"/>.</returns>
        IList<ComposableTokenType> Tokenize(IContentReader reader, IComposableTokenizer initiator);
    }
}
