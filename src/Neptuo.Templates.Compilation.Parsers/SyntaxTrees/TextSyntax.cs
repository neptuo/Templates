using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class TextSyntax : ISyntaxNode
    {
        public IReadOnlyList<ComposableToken> Tokens { get; private set; }

        public ComposableToken TextToken { get; private set; }

        public TextSyntax(ComposableToken textToken)
        {
            TextToken = textToken;

            List<ComposableToken> tokens = new List<ComposableToken>();
            if (textToken != null)
                tokens.Add(textToken);

            Tokens = tokens;
        }

        public TextSyntax WithText(ComposableToken textToken)
        {
            return new TextSyntax(textToken);
        }


        public static TextSyntax New()
        {
            return new TextSyntax(null);
        }
    }
}
