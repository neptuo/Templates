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
        private readonly Dictionary<TokenType, IComposableSyntaxBuilder> storage = new Dictionary<TokenType, IComposableSyntaxBuilder>();

        public SyntaxBuilderCollection Add(TokenType tokenType, IComposableSyntaxBuilder builder)
        {
            Ensure.NotNull(tokenType, "tokenType");
            Ensure.NotNull(builder, "builder");
            storage[tokenType] = builder;
            return this;
        }

        public ISyntaxNode Build(IList<Token> tokens, int startIndex)
        {
            SyntaxNodeCollection result = new SyntaxNodeCollection();
            for (int i = startIndex; i < tokens.Count; )
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
            IComposableSyntaxBuilder builder;
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
