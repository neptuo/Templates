using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class AngleAttributeNodeVisitor : NodeVisitorBase<AngleAttributeNode>
    {
        protected override void Visit(AngleAttributeNode node, INodeProcessor processor)
        {
            if (node.Name != null)
                processor.Process(node.Name);

            if (node.Value != null)
                processor.Process(node.Value);
        }
    }
}
