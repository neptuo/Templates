using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class CurlyDefaultAttributeNodeVisitor : NodeVisitorBase<CurlyDefaultAttributeNode>
    {
        protected override void Visit(CurlyDefaultAttributeNode node, INodeProcessor processor)
        {
            if (node.Value != null)
                processor.Process(node.Value);
        }
    }
}
