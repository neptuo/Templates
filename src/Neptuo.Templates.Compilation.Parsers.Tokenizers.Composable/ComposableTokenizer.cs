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
        private readonly ComposableTokenTypeCollection supportedTokens;
        private readonly List<IComposableTokenizer> tokenizers;

        /// <summary>
        /// Creates new instance of tokenizer.
        /// </summary>
        public ComposableTokenizer()
            : this(new List<ComposableTokenType>() { ComposableTokenType.Error, ComposableTokenType.Text })
        { }

        /// <summary>
        /// Creates new instance with initially supported tokens.
        /// </summary>
        /// <param name="supportedTokens">Initially supported tokens.</param>
        public ComposableTokenizer(IEnumerable<ComposableTokenType> supportedTokens)
        {
            this.supportedTokens = new ComposableTokenTypeCollection(supportedTokens);
            this.tokenizers = new List<IComposableTokenizer>() { new PlainComposableTokenizer() };
        }

        public IList<ComposableToken> Tokenize(IContentReader reader, ITokenizerContext context)
        {
            ComposableTokenizerContext superContext = new ComposableTokenizerContext(reader, context, tokenizers);
            Tokenize(superContext);
            return superContext.Tokens;
        }

        private void Tokenize(ComposableTokenizerContext context)
        {
            while (context.Reader.Next())
            {
                context.CurrentText.Append(context.Reader.Current);
                context.Accept(context.Reader.Current);

                //TODO: Process character.
                // 1) Call all registered tokenizers (and add them to temporal list).
                // 2) When first tokenizer exports token, kill all others (remove all others from list).
                // 3) Use selected tokenizer until it calls for 'activate other tokenizers', with stop token/char (when other tokiners should stop).
                // 4) Stop tokenizer when stop token/char is found OR end of file is reached.

            }

            context.Finalize();
        }
    }
}
