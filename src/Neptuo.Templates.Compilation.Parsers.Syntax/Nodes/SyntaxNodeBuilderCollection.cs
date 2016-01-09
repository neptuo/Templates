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
            TokenListReader reader = new TokenListReader(tokens, true);
            while (reader.Next())
            {
                Token token = reader.Current;
                ISyntaxNode node = BuildNext(reader);
                result.Add(node);
            }

            return result;
        }

        public ISyntaxNode BuildNext(TokenListReader reader)
        {
            ISyntaxNodeBuilder builder;
            if (storage.TryGetValue(reader.Current.Type, out builder))
            {
                ISyntaxNode node = builder.Build(reader, this);
                return node;
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, storage.Keys.ToArray());
            }
        }
    }
}
