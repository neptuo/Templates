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
        public ISyntaxNode Build(IList<Token> tokens, int startIndex, ISyntaxNodeBuilderContext context)
        {
            Token token = tokens[startIndex];
            if (token.Type == TokenType.Literal)
            {
                return new LiteralSyntax
                {
                    TextToken = token
                };
            }

            throw new NotImplementedException();
        }
    }
}
