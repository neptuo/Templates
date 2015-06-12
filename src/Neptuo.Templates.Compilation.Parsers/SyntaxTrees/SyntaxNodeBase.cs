using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public abstract class SyntaxNodeBase : ISyntaxNode
    {
        private List<ComposableToken> tokens = new List<ComposableToken>();

        public IReadOnlyList<ComposableToken> Tokens
        {
            get { return tokens; }
        }

        protected void AddNullableTokens(params ISyntaxNode[] nodes)
        {
            AddNullableTokens((IEnumerable<ISyntaxNode>)nodes);
        }

        protected void AddNullableTokens(IEnumerable<ISyntaxNode> nodes)
        {
            foreach (ISyntaxNode node in nodes)
            {
                if (node != null)
                    AddNullableTokens(node.Tokens);
            }
        }

        protected void AddNullableTokens(params ComposableToken[] tokens)
        {
            AddNullableTokens((IEnumerable<ComposableToken>)tokens);
        }

        protected void AddNullableTokens(IEnumerable<ComposableToken> tokens)
        {
            foreach (ComposableToken token in tokens)
            {
                if (token != null)
                    this.tokens.Add(token);
            }
        }
    }
}
