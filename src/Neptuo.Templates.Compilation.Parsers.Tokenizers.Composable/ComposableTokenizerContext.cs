using Neptuo.Activators;
using Neptuo.ComponentModel;
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

        public ComposableTokenizerContext(List<IComposableTokenizer> tokenizers, ITokenizerContext context)
            : base(context.DependencyProvider, context.Errors)
        {
            Ensure.NotNull(tokenizers, "tokenizers");
            this.tokenizers = tokenizers;
        }

        public IList<ComposableToken> Tokenize(IContentReader reader, IComposableTokenizer initiator)
        {
            IActivator<IContentReader> factory = new ContentFactory(reader);
            foreach (IComposableTokenizer tokenizer in tokenizers)
            {
                if (tokenizer != initiator)
                {
                    IList<ComposableToken> tokens = tokenizer.Tokenize(factory.Create(), this);
                    if (tokens.Any())
                        return tokens;
                }
            }

            //TODO: Return empty collection.
            throw Ensure.Exception.NotImplemented();
        }
    }
}
