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
        /// Processes <paramref name="decorator"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="reader">Input content reader.</param>
        /// <param name="currentInfo">Offset info.</param>
        /// <returns>List of syntactic tokens from <paramref name="decorator"/>.</returns>
        IList<ComposableToken> Tokenize(IContentReader reader, ICurrentInfoAware currentInfo);
    }
}
