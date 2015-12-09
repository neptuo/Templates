using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class SyntaxVisitor : ISyntaxNodeVisitor
    {
        private readonly Dictionary<Type, ISyntaxNodeVisitor> storage = new Dictionary<Type, ISyntaxNodeVisitor>();

        public SyntaxVisitor Add(Type syntaxType, ISyntaxNodeVisitor visitor)
        {
            Ensure.NotNull(syntaxType, "syntaxType");
            Ensure.NotNull(visitor, "visitor");
            storage[syntaxType] = visitor;
            return this;
        }

        public void Visit(ISyntaxNode node, ISyntaxNodeProcessor processor)
        {
            Ensure.NotNull(node, "node");
            Ensure.NotNull(processor, "processor");

        }

        private class SyntaxProcessor : ISyntaxNodeProcessor
        {
            public void Process(ISyntaxNode node, ISyntaxNodeProcessor nextProcessor)
            {
                throw new NotImplementedException();
            }
        }
    }
}
