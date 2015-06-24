using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class SyntaxBuilderCollection : ISyntaxBuilder, IComposableSyntaxBuilderContext
    {
        private readonly Dictionary<ComposableTokenType, IComposableSyntaxBuilder> storage = new Dictionary<ComposableTokenType, IComposableSyntaxBuilder>();

        public SyntaxBuilderCollection Add(ComposableTokenType tokenType, IComposableSyntaxBuilder builder)
        {
            Ensure.NotNull(tokenType, "tokenType");
            Ensure.NotNull(builder, "builder");
            storage[tokenType] = builder;
            return this;
        }

        public ISyntaxNode Build(IList<ComposableToken> tokens, int startIndex)
        {
            SyntaxNodeCollection result = new SyntaxNodeCollection();
            for (int i = startIndex; i < tokens.Count; )
            {
                ComposableToken token = tokens[i];
                IComposableSyntaxBuilder builder;
                if (storage.TryGetValue(token.Type, out builder))
                {
                    ISyntaxNode node = builder.Build(tokens, i, this);
                    i += node.GetTokens().Count();
                    result.Nodes.Add(node);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
