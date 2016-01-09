using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class LiteralSyntaxNodeBuilder : ISyntaxNodeBuilder
    {
        public ISyntaxNode Build(TokenListReader reader, ISyntaxNodeBuilderContext context)
        {
            if (reader.Current.Type == TokenType.Literal)
            {
                return new LiteralSyntax()
                    .WithTextToken(reader.Current);
            }

            throw new InvalidTokenTypeException(reader.Current, TokenType.Literal);
        }
    }
}
