using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class TextSyntax : SyntaxNodeBase<TextSyntax>
    {
        public ComposableToken TextToken { get; set; }

        protected override TextSyntax CloneInternal()
        {
            return new TextSyntax
            {
                TextToken = TextToken
            };
        }

        protected override IEnumerable<ComposableToken> GetTokensInternal()
        {
            List<ComposableToken> result = new List<ComposableToken>();

            if (TextToken != null)
                result.Add(TextToken);

            return result;
        }
    }
}
