using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers
{
    public class SyntaxBuilderCollection : ISyntaxBuilder
    {
        private readonly Dictionary<ComposableTokenType, ISyntaxBuilder> storage = new Dictionary<ComposableTokenType, ISyntaxBuilder>();

        public SyntaxBuilderCollection Add(ComposableTokenType tokenType, ISyntaxBuilder builder)
        {
            Ensure.NotNull(tokenType, "tokenType");
            Ensure.NotNull(builder, "builder");
            storage[tokenType] = builder;
            return this;
        }

        public ISyntaxNode Build(IList<ComposableToken> tokens, int startIndex)
        {
            SyntaxtCollection result = new SyntaxtCollection();
            for (int i = startIndex; i < tokens.Count; i++)
            {
                ComposableToken token =tokens[i];
                ISyntaxBuilder builder;
                if(storage.TryGetValue(token.Type, out builder))
                {
                    ISyntaxNode node = builder.Build(tokens, i);
                    i += node.Tokens.Count - 1;
                    result.Nodes.Add(node);
                }
            }

            return result;
        }
    }
}
