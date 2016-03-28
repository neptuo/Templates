using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class LiteralNodeBuilder : INodeBuilder
    {
        public INode Build(TokenReader reader, INodeBuilderContext context)
        {
            if (reader.Current.Type == TokenType.Literal)
            {
                return new LiteralNode()
                    .WithTextToken(reader.Current);
            }

            throw new InvalidTokenTypeException(reader.Current, TokenType.Literal);
        }
    }
}
