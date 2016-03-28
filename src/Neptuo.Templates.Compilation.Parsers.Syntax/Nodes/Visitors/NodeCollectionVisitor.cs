using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class NodeCollectionVisitor : NodeVisitorBase<NodeCollection>
    {
        protected override void Visit(NodeCollection node, INodeProcessor processor)
        {
            foreach (INode item in node.Nodes)
                processor.Process(item);
        }
    }
}
