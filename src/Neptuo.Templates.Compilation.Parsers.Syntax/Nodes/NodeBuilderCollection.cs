using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class NodeBuilderCollection : INodeFactory, INodeBuilderContext
    {
        private readonly Dictionary<TokenType, INodeBuilder> storage = new Dictionary<TokenType, INodeBuilder>();

        public NodeBuilderCollection Add(TokenType tokenType, INodeBuilder builder)
        {
            Ensure.NotNull(tokenType, "tokenType");
            Ensure.NotNull(builder, "builder");
            storage[tokenType] = builder;
            return this;
        }

        public INode Create(IList<Token> tokens)
        {
            NodeCollection result = new NodeCollection();
            TokenReader reader = new TokenReader(tokens, true);
            while (reader.Next())
            {
                Token token = reader.Current;
                INode node = BuildNext(reader);
                result.Add(node);
            }

            return result;
        }

        public INode BuildNext(TokenReader reader)
        {
            INodeBuilder builder;
            if (storage.TryGetValue(reader.Current.Type, out builder))
            {
                INode node = builder.Build(reader, this);
                return node;
            }
            else
            {
                throw new InvalidTokenTypeException(reader.Current, storage.Keys.ToArray());
            }
        }
    }
}
