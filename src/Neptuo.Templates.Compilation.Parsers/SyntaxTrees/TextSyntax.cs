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
        public ComposableToken TextToken { get; private set; }

        public TextSyntax(ComposableToken textToken, IEnumerable<ComposableToken> leadingTrivia, IEnumerable<ComposableToken> trailingTrivia)
            : base(leadingTrivia, trailingTrivia)
        {
            TextToken = textToken;

            AddNullableLeadingTrivia();
            AddNullableTokens(textToken);
            AddNullableTrailingTrivia();
        }

        public override TextSyntax Clone()
        {
            return new TextSyntax(TextToken, LeadingTrivia, TrailingTrivia);
        }

        public TextSyntax WithText(ComposableToken textToken)
        {
            return new TextSyntax(textToken, LeadingTrivia, TrailingTrivia);
        }


        public static TextSyntax New()
        {
            return new TextSyntax(null, null, null);
        }
    }
}
