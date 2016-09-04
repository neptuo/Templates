using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class NodeCollection : NodeBase<NodeCollection>
    {
        public IList<INode> Nodes { get; private set; }

        public NodeCollection()
        {
            Nodes = new List<INode>();
        }

        public NodeCollection Add(INode node)
        {
            Nodes.Add(node);

            if (node != null)
                node.Parent = this;

            return this;
        }

        protected override NodeCollection CloneInternal()
        {
            NodeCollection result = new NodeCollection();

            foreach (INode node in Nodes)
            {
                ICloneable<INode> cloneableNode = node as ICloneable<INode>;
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
