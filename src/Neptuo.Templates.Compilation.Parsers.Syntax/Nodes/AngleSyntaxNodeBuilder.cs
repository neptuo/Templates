using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class AngleSyntaxNodeBuilder : ISyntaxNodeBuilder
    {
        public ISyntaxNode Build(IList<Token> tokens, int startIndex, ISyntaxNodeBuilderContext context)
        {
            CurlySyntax result = new CurlySyntax();

            TokenListReader reader = new TokenListReader(tokens, startIndex);
            Token token = reader.Current;

            while (token.Type == CurlyTokenType.Whitespace)
            {
                result.LeadingTrivia.Add(token);
                if (!reader.Next())
                    throw new NotImplementedException();

                token = reader.Current;
            }

            if (token.Type == CurlyTokenType.OpenBrace)
            {
                result.OpenToken = token;
                return BuildName(reader, result, context);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
