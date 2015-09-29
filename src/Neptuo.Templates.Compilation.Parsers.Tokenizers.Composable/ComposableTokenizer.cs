﻿using Neptuo.Activators;
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
    public class ComposableTokenizer : ITokenizer<ComposableToken>, IComposableTokenizerCollection
    {
        private readonly ComposableTokenTypeCollection supportedTokens;
        private readonly List<IComposableTokenizer> tokenizers;

        /// <summary>
        /// Creates new instance of tokenizer.
        /// </summary>
        public ComposableTokenizer()
            : this(new List<ComposableTokenType>() { ComposableTokenType.Text, ComposableTokenType.Whitespace })
        { }

        /// <summary>
        /// Creates new instance with initially supported tokens.
        /// </summary>
        /// <param name="supportedTokens">Initially supported tokens.</param>
        public ComposableTokenizer(IEnumerable<ComposableTokenType> supportedTokens)
        {
            this.supportedTokens = new ComposableTokenTypeCollection(supportedTokens);
            this.tokenizers = new List<IComposableTokenizer>() { };//new PlainComposableTokenizer() };
        }

        public IList<ComposableToken> Tokenize(IContentReader reader, ITokenizerContext context)
        {
            IFactory<IContentReader> factory = new ContentFactory(reader);
            foreach (IComposableTokenizer tokenizer in tokenizers)
            {
                IComposableTokenizerContext superContext = new ComposableTokenizerContext(tokenizers, tokenizer, context, new Stack<IComposableTokenizer>());
                ContentDecorator decorator = new ContentDecorator(factory.Create());

                IList<ComposableToken> tokens = tokenizer.Tokenize(decorator, superContext);
                if (tokens.Any())
                    return tokens;
            }

            return new List<ComposableToken>();
        }

        public IComposableTokenizerCollection Add(IComposableTokenizer tokenizer)
        {
            Ensure.NotNull(tokenizer, "tokenizer");
            tokenizers.Add(tokenizer);

            IComposableTokenTypeProvider provider = tokenizer as IComposableTokenTypeProvider;
            if (provider != null)
                supportedTokens.AddRange(provider.GetSupportedTokenTypes());

            return this;
        }
    }
}