using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public abstract class SyntaxNodeVisitorBase<T> : ISyntaxNodeVisitor
        where T : ISyntaxNode
    {
        public void Visit(ISyntaxNode node, ISyntaxNodeProcessor processor)
        {
            T targetNode = (T)node;
            Visit(targetNode, processor);
        }

        protected abstract void Visit(T node, ISyntaxNodeProcessor processor);
    }
}
