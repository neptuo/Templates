using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class SyntaxCollection : SyntaxNodeBase<SyntaxCollection>
    {
        public IList<ISyntaxNode> Nodes { get; private set; }

        public SyntaxCollection()
        {
            Nodes = new List<ISyntaxNode>();
        }

        public SyntaxCollection Add(ISyntaxNode node)
        {
            Nodes.Add(node);

            if (node != null)
                node.Parent = this;

            return this;
        }

        protected override SyntaxCollection CloneInternal()
        {
            SyntaxCollection result = new SyntaxCollection();

            foreach (ISyntaxNode node in Nodes)
            {
                ICloneable<ISyntaxNode> cloneableNode = node as ICloneable<ISyntaxNode>;
                if (cloneableNode != null)
                    result.Add(cloneableNode.Clone());
                else
                    throw Ensure.Condition.NotCloneable(node);
            }

            return result;
        }

        protected override IEnumerable<Token> GetTokensInternal()
        {
            return Nodes.SelectMany(n => n.GetTokens());
        }
    }
}
