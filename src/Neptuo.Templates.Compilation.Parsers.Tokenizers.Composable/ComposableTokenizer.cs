using Neptuo.Activators;
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
    public class ComposableTokenizer : ITokenizer, ITokenizerCollection
    {
        private readonly TokenTypeCollection supportedTokens;
        private readonly List<TokenizerBase> tokenizers;

        /// <summary>
        /// Creates new instance of tokenizer.
        /// </summary>
        public ComposableTokenizer()
            : this(new List<TokenType>() { TokenType.Text, TokenType.Whitespace })
        { }

        /// <summary>
        /// Creates new instance with initially supported tokens.
        /// </summary>
        /// <param name="supportedTokens">Initially supported tokens.</param>
        public ComposableTokenizer(IEnumerable<TokenType> supportedTokens)
        {
            this.supportedTokens = new TokenTypeCollection(supportedTokens);
            this.tokenizers = new List<TokenizerBase>() { };//new PlainComposableTokenizer() };
        }

        public IList<Token> Tokenize(IContentReader reader, ITokenizerContext context)
        {
            IFactory<IContentReader> factory = new ContentFactory(reader);
            foreach (TokenizerBase tokenizer in tokenizers)
            {
                IComposableTokenizerContext superContext = new ComposableTokenizerContext(tokenizers, tokenizer, context, new Stack<TokenizerBase>());
                ContentDecorator decorator = new ContentDecorator(factory.Create());

                IList<Token> tokens = tokenizer.Tokenize(decorator, superContext);
                if (tokens.Any())
                    return tokens;
            }

            return new List<Token>();
        }

        public ITokenizerCollection Add(TokenizerBase tokenizer)
        {
            Ensure.NotNull(tokenizer, "tokenizer");
            tokenizers.Add(tokenizer);

            ITokenTypeProvider provider = tokenizer as ITokenTypeProvider;
            if (provider != null)
                supportedTokens.AddRange(provider.GetSupportedTokenTypes());

            return this;
        }
    }
}
