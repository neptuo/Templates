using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class TextSyntaxBuilder : ISyntaxBuilder
    {
        public ISyntaxNode Build(IList<ComposableToken> tokens, int startIndex)
        {
            ComposableToken token = tokens[startIndex];
            if (token.Type == ComposableTokenType.Text)
                return TextSyntax.New().WithText(token);

            throw new NotImplementedException();
        }
    }
}
