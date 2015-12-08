using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class SyntaxCollectionVisitor : SyntaxNodeVisitorBase<SyntaxCollection>
    {
        protected override void Visit(SyntaxCollection node, ISyntaxNodeProcessor processor)
        {
            foreach (ISyntaxNode item in node.Nodes)
                processor.Process(item);
        }
    }
}
