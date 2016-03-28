using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class NodeVisitor : INodeVisitor
    {
        private readonly Dictionary<Type, INodeVisitor> storage = new Dictionary<Type, INodeVisitor>();

        public NodeVisitor Add(Type syntaxType, INodeVisitor visitor)
        {
            Ensure.NotNull(syntaxType, "syntaxType");
            Ensure.NotNull(visitor, "visitor");
            storage[syntaxType] = visitor;
            return this;
        }

        public void Visit(INode node, INodeProcessor processor)
        {
            Ensure.NotNull(node, "node");
            Ensure.NotNull(processor, "processor");

            NodeProcessor context = new NodeProcessor(processor, OnVisitNode);
            context.Process(node);
        }

        private void OnVisitNode(INode node, NodeProcessor context)
        {
            Type nodeType = node.GetType();
            INodeVisitor visitor;
            if (storage.TryGetValue(nodeType, out visitor))
                visitor.Visit(node, context);
            else
                throw Ensure.Exception.NotSupported("Unsupported node type '{0}'.", nodeType.FullName);
        }

        private class NodeProcessor : INodeProcessor
        {
            private readonly INodeProcessor innerProcessor;
            private readonly Action<INode, NodeProcessor> handler;

            public NodeProcessor(INodeProcessor innerProcessor, Action<INode, NodeProcessor> handler)
            {
                this.innerProcessor = innerProcessor;
                this.handler = handler;
            }

            public bool Process(INode node)
            {
                bool result = innerProcessor.Process(node);
                if (result)
                    handler(node, this);

                return result;
            }
        }
    }
}
