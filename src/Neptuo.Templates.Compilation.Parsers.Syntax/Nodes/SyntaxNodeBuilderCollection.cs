using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class SyntaxNodeBuilderCollection : ISyntaxNodeFactory, ISyntaxNodeBuilderContext
    {
        private readonly Dictionary<TokenType, ISyntaxNodeBuilder> storage = new Dictionary<TokenType, ISyntaxNodeBuilder>();

        public SyntaxNodeBuilderCollection Add(TokenType tokenType, ISyntaxNodeBuilder builder)
        {
            Ensure.NotNull(tokenType, "tokenType");
            Ensure.NotNull(builder, "builder");
            storage[tokenType] = builder;
            return this;
        }

        public ISyntaxNode Create(IList<Token> tokens)
        {
            SyntaxCollection result = new SyntaxCollection();
            for (int i = 0; i < tokens.Count; )
            {
                Token token = tokens[i];
                ISyntaxNode node = BuildNext(tokens, i);
                i += node.GetTokens().Count();
                result.Nodes.Add(node);
            }

            return result;
        }

        public ISyntaxNode BuildNext(IList<Token> tokens, int startIndex)
        {
            Token token = tokens[startIndex];
            ISyntaxNodeBuilder builder;
            if (storage.TryGetValue(token.Type, out builder))
            {
                ISyntaxNode node = builder.Build(tokens, startIndex, this);
                return node;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
