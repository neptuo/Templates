using Neptuo.Activators;
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

        public ComposableTokenizerContext(List<IComposableTokenizer> tokenizers, IComposableTokenizer targetTokenizer, ITokenizerContext context)
            : base(context.DependencyProvider, context.Errors)
        {
            Ensure.NotNull(tokenizers, "tokenizers");
            Ensure.NotNull(targetTokenizer, "targetTokenizer");
            this.tokenizers = tokenizers;
            this.targetTokenizer = targetTokenizer;
        }

        public IList<ComposableToken> Tokenize(IContentReader reader, ICurrentInfoAware currentInfo)
        {
            IActivator<IContentReader> factory = new ContentFactory(reader);
            foreach (IComposableTokenizer tokenizer in tokenizers)
            {
                if (tokenizer != targetTokenizer)
                {
                    IComposableTokenizerContext superContext = new ComposableTokenizerContext(tokenizers, tokenizer, this);
                    ContentDecorator decorator = new ContentDecorator(factory.Create(), currentInfo.Position, currentInfo.LineIndex, currentInfo.ColumnIndex);

                    IList<ComposableToken> tokens = tokenizer.Tokenize(decorator, new ComposableTokenizerContext(tokenizers, tokenizer, this));
                    if (tokens.Any())
                        return tokens;
                }
            }

            return new List<ComposableToken>();
        }
    }
}
