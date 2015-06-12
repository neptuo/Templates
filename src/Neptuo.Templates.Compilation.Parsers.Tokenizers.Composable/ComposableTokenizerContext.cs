﻿using Neptuo.Activators;
using Neptuo.ComponentModel;
using Neptuo.ComponentModel.TextOffsets;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Default implementation of <see cref="IComposableTokenizerContext"/>
    /// </summary>
    internal class ComposableTokenizerContext : DefaultTokenizerContext, IComposableTokenizerContext
    {
        private readonly List<IComposableTokenizer> tokenizers;
        private readonly IComposableTokenizer targetTokenizer;
        private readonly Stack<IComposableTokenizer> currentTokenizers;

        public ComposableTokenizerContext(List<IComposableTokenizer> tokenizers, IComposableTokenizer targetTokenizer, ITokenizerContext context, Stack<IComposableTokenizer> currentTokenizers)
            : base(context.DependencyProvider, context.Errors)
        {
            Ensure.NotNull(tokenizers, "tokenizers");
            Ensure.NotNull(targetTokenizer, "targetTokenizer");
            Ensure.NotNull(currentTokenizers, "currentTokenizers");
            this.tokenizers = tokenizers;
            this.targetTokenizer = targetTokenizer;
            this.currentTokenizers = currentTokenizers;
        }

        public IList<ComposableToken> Tokenize(IContentReader reader, ICurrentInfoAware currentInfo)
        {
            currentTokenizers.Push(targetTokenizer);
            IList<ComposableToken> tokens = new List<ComposableToken>();
            IActivator<IContentReader> factory = new ContentFactory(reader);
            foreach (IComposableTokenizer tokenizer in tokenizers)
            {
                if (!currentTokenizers.Contains(tokenizer))
                {
                    IComposableTokenizerContext superContext = new ComposableTokenizerContext(tokenizers, tokenizer, this, currentTokenizers);
                    ContentDecorator decorator = new ContentDecorator(factory.Create(), currentInfo.Position, currentInfo.LineIndex, currentInfo.ColumnIndex);

                    tokens = tokenizer.Tokenize(decorator, new ComposableTokenizerContext(tokenizers, tokenizer, this, currentTokenizers));
                    if (tokens.Any())
                        break;
                }
            }

            currentTokenizers.Pop();
            return tokens;
        }
    }
}
