using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class CurlyDefaultAttributeSyntaxVisitor : SyntaxNodeVisitorBase<CurlyDefaultAttributeSyntax>
    {
        protected override void Visit(CurlyDefaultAttributeSyntax node, ISyntaxNodeProcessor processor)
        {
            if (node.Value != null)
                processor.Process(node.Value);
        }
    }
}
