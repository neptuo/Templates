using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class TokenAtPositionFinder : ISyntaxNodeProcessor
    {
        public int Position { get; private set; }
        public ISyntaxNode BestMatch { get; private set; }

        public TokenAtPositionFinder(int position)
        {
            Ensure.PositiveOrZero(position, "position");
            Position = position;
        }

        public void Process(ISyntaxNode node)
        {
            bool isMatch = node.GetTokens().Any(t => t.TextSpan.StartIndex <= Position && t.TextSpan.StartIndex + t.TextSpan.Length >= Position);
            if (isMatch)
                BestMatch = node;
        }
    }
}
