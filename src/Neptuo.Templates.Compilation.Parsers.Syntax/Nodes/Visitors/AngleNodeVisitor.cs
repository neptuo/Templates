using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class AngleNodeVisitor : NodeVisitorBase<AngleNode>
    {
        protected override void Visit(AngleNode node, INodeProcessor processor)
        {
            if (node.Name != null)
                processor.Process(node.Name);

            foreach (AngleAttributeNode attribute in node.Attributes)
                processor.Process(attribute);
        }
    }
}
