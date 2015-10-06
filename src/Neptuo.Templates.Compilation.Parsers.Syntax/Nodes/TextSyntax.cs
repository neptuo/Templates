using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class TextSyntax : SyntaxNodeBase<TextSyntax>
    {
        public Token TextToken { get; set; }

        protected override TextSyntax CloneInternal()
        {
            return new TextSyntax
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
