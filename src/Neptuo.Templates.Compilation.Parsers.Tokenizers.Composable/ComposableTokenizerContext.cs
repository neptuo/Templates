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
    /// Default implementation of <see cref="IComposableTokenizerContext"/>
    /// </summary>
    internal class ComposableTokenizerContext : DefaultTokenizerContext, IComposableTokenizerContext
    {
        private readonly List<TokenizerBase> tokenizers;
        private readonly TokenizerBase targetTokenizer;
        private readonly Stack<TokenizerBase> currentTokenizers;

        public ComposableTokenizerContext(List<TokenizerBase> tokenizers, TokenizerBase targetTokenizer, ITokenizerContext context, Stack<TokenizerBase> currentTokenizers)
            : base(context.DependencyProvider, context.Errors)
        {
            Ensure.NotNull(tokenizers, "tokenizers");
            Ensure.NotNull(targetTokenizer, "targetTokenizer");
            Ensure.NotNull(currentTokenizers, "currentTokenizers");
            this.tokenizers = tokenizers;
            this.targetTokenizer = targetTokenizer;
            this.currentTokenizers = currentTokenizers;
        }

        public IList<Token> Tokenize(IContentReader reader, ICurrentInfoAware currentInfo)
        {
            currentTokenizers.Push(targetTokenizer);
            IList<Token> tokens = new List<Token>();
            IFactory<IContentReader> factory = new ContentFactory(reader);
            foreach (TokenizerBase tokenizer in tokenizers)
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
