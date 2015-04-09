using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Extensible implementation of <see cref="ITokenizer{T}"/>.
    /// </summary>
    public class ComposableTokenizer : ITokenizer<ComposableToken>
    {
        private readonly ComposableTokenCollection supportedTokens;

        /// <summary>
        /// Creates new instance of tokenizer.
        /// </summary>
        public ComposableTokenizer()
            : this(Enumerable.Empty<ComposableToken>())
        { }

        /// <summary>
        /// Creates new instance with initially supported tokens.
        /// </summary>
        /// <param name="supportedTokens">Initially supported tokens.</param>
        public ComposableTokenizer(IEnumerable<ComposableToken> supportedTokens)
        {
            this.supportedTokens = new ComposableTokenCollection(supportedTokens);
        }

        public IList<ComposableToken> Tokenize(IContentReader reader, ITokenizerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
