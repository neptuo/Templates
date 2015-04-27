using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public interface IComposableTokenizer
    {
        /// <summary>
        /// Processes <paramref name="decorator"/> and creates list of syntactic tokens.
        /// </summary>
        /// <param name="decorator">Input content decorator.</param>
        /// <param name="context">Context for input processing.</param>
        /// <returns>List of syntactic tokens from <paramref name="decorator"/>.</returns>
        IList<ComposableToken> Tokenize(ContentDecorator decorator, IComposableTokenizerContext context);
    }
}
