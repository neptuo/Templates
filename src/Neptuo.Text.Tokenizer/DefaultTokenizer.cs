using Neptuo.Activators;
using Neptuo.Text.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Text
{
    /// <summary>
    /// Extensible implementation of <see cref="ITokenizer{T}"/>.
    /// </summary>
    public class DefaultTokenizer : ITokenizer, ITokenBuilderCollection
    {
        private readonly TokenTypeCollection supportedTokens;
        private readonly List<ITokenBuilder> tokenizers;

        /// <summary>
        /// Creates new instance of tokenizer.
        /// </summary>
        public DefaultTokenizer()
            : this(new List<TokenType>() { TokenType.Literal, TokenType.Whitespace })
        { }

        /// <summary>
        /// Creates new instance with initially supported tokens.
        /// </summary>
        /// <param name="supportedTokens">Initially supported tokens.</param>
        public DefaultTokenizer(IEnumerable<TokenType> supportedTokens)
        {
            Ensure.NotNull(supportedTokens, "supportedTokens");
            this.supportedTokens = new TokenTypeCollection(supportedTokens);
            this.tokenizers = new List<ITokenBuilder>();
        }

        public IList<Token> Tokenize(IContentReader reader, ITokenizerContext context)
        {
            IFactory<IContentReader> factory = new ContentFactory(reader);
            foreach (TokenBuilderBase tokenizer in tokenizers)
            {
                ITokenBuilderContext superContext = new DefaultTokenBuilderContext(tokenizers, tokenizer, context, new Stack<ITokenBuilder>());
                ContentDecorator decorator = new ContentDecorator(factory.Create());

                IList<Token> tokens = tokenizer.Tokenize(decorator, superContext);
                if (tokens.Any())
                    return tokens;
            }

            return new List<Token>();
        }

        public ITokenBuilderCollection Add(ITokenBuilder tokenBuilder)
        {
            Ensure.NotNull(tokenBuilder, "tokenBuilder");
            tokenizers.Add(tokenBuilder);

            ITokenTypeProvider provider = tokenBuilder as ITokenTypeProvider;
            if (provider != null)
                supportedTokens.AddRange(provider.GetSupportedTokenTypes());

            return this;
        }

        public IEnumerable<ITokenBuilder> EnumerateBuilders()
        {
            return tokenizers;
        }
    }
}
