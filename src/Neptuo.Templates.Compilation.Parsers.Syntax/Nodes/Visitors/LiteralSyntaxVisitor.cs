using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class LiteralSyntaxVisitor : SyntaxNodeVisitorBase<LiteralSyntax>
    {
        protected override void Visit(LiteralSyntax node, ISyntaxNodeProcessor processor)
        {
            processor.Process(node);
        }
    }
}
