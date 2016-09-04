using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class LiteralNode : NodeBase<LiteralNode>
    {
        public Token TextToken { get; private set; }

        public LiteralNode WithTextToken(Token textToken)
        {
            TextToken = textToken;
            return this;
        }

        protected override LiteralNode CloneInternal()
        {
            return new LiteralNode
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
