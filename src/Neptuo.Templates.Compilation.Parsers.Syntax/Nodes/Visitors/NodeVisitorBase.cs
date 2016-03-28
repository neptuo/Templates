using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public abstract class NodeVisitorBase<T> : INodeVisitor
        where T : INode
    {
        public void Visit(INode node, INodeProcessor processor)
        {
            T targetNode = (T)node;
            Visit(targetNode, processor);
        }

        protected abstract void Visit(T node, INodeProcessor processor);
    }
}
