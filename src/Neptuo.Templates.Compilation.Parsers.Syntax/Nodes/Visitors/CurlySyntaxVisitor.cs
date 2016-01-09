using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class CurlySyntaxVisitor : SyntaxNodeVisitorBase<CurlySyntax>
    {
        protected override void Visit(CurlySyntax node, ISyntaxNodeProcessor processor)
        {
            if (node.Name != null)
                processor.Process(node.Name);

            foreach (CurlyDefaultAttributeSyntax attribute in node.DefaultAttributes)
                processor.Process(attribute);

            foreach (CurlyAttributeSyntax attribute in node.Attributes)
                processor.Process(attribute);
        }
    }
}
