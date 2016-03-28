using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class LiteralNodeVisitor : NodeVisitorBase<LiteralNode>
    {
        protected override void Visit(LiteralNode node, INodeProcessor processor)
        { }
    }
}
