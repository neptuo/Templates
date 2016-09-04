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
    /// Default implementation of <see cref="ITokenBuilderContext"/>
    /// </summary>
    internal class DefaultTokenBuilderContext : DefaultTokenizerContext, ITokenBuilderContext
    {
        private readonly List<ITokenBuilder> tokenizers;
        private readonly ITokenBuilder targetTokenizer;
        private readonly Stack<ITokenBuilder> currentTokenizers;

        public DefaultTokenBuilderContext(List<ITokenBuilder> tokenizers, ITokenBuilder targetTokenizer, ITokenizerContext context, Stack<ITokenBuilder> currentTokenizers)
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
            foreach (TokenBuilderBase tokenizer in tokenizers)
            {
                if (!currentTokenizers.Contains(tokenizer))
                {
                    ITokenBuilderContext superContext = new DefaultTokenBuilderContext(tokenizers, tokenizer, this, currentTokenizers);
                    ContentDecorator decorator = new ContentDecorator(factory.Create(), currentInfo.Position, currentInfo.LineIndex, currentInfo.ColumnIndex);

                    tokens = tokenizer.Tokenize(decorator, new DefaultTokenBuilderContext(tokenizers, tokenizer, this, currentTokenizers));
                    if (tokens.Any())
                        break;
                }
            }

            currentTokenizers.Pop();
            return tokens;
        }
    }
}
