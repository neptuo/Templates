using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class CurlyNodeVisitor : NodeVisitorBase<CurlyNode>
    {
        protected override void Visit(CurlyNode node, INodeProcessor processor)
        {
            if (node.Name != null)
                processor.Process(node.Name);

            foreach (CurlyDefaultAttributeNode attribute in node.DefaultAttributes)
                processor.Process(attribute);

            foreach (CurlyAttributeNode attribute in node.Attributes)
                processor.Process(attribute);
        }
    }
}
