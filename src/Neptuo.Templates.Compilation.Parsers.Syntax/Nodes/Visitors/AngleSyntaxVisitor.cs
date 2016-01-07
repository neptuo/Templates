using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class AngleSyntaxVisitor : SyntaxNodeVisitorBase<AngleSyntax>
    {
        protected override void Visit(AngleSyntax node, ISyntaxNodeProcessor processor)
        {
            if (node.Name != null)
                processor.Process(node.Name);

            foreach (AngleAttributeSyntax attribute in node.Attributes)
                processor.Process(attribute);
        }
    }
}
