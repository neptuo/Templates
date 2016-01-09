using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class LiteralSyntax : SyntaxNodeBase<LiteralSyntax>
    {
        public Token TextToken { get; private set; }

        public LiteralSyntax WithTextToken(Token textToken)
        {
            TextToken = textToken;
            return this;
        }

        protected override LiteralSyntax CloneInternal()
        {
            return new LiteralSyntax
            {
                TextToken = TextToken
            };
        }

        protected override IEnumerable<Token> GetTokensInternal()
        {
            List<Token> result = new List<Token>();

            if (TextToken != null)
                result.Add(TextToken);

            return result;
        }
    }
}
