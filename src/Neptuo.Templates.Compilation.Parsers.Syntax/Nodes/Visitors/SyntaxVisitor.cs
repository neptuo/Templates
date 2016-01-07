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

            SyntaxProcessor context = new SyntaxProcessor(processor, OnVisitNode);
            context.Process(node);
        }

        private void OnVisitNode(ISyntaxNode node, SyntaxProcessor context)
        {
            Type nodeType = node.GetType();
            ISyntaxNodeVisitor visitor;
            if (storage.TryGetValue(nodeType, out visitor))
                visitor.Visit(node, context);
            else
                throw Ensure.Exception.NotSupported("Unsupported node type '{0}'.", nodeType.FullName);
        }

        private class SyntaxProcessor : ISyntaxNodeProcessor
        {
            private readonly ISyntaxNodeProcessor innerProcessor;
            private readonly Action<ISyntaxNode, SyntaxProcessor> handler;

            public SyntaxProcessor(ISyntaxNodeProcessor innerProcessor, Action<ISyntaxNode, SyntaxProcessor> handler)
            {
                this.innerProcessor = innerProcessor;
                this.handler = handler;
            }

            public void Process(ISyntaxNode node)
            {
                innerProcessor.Process(node);
                handler(node, this);
            }
        }
    }
}
