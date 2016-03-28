using Neptuo.Activators;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Templates.VisualStudio.IntelliSense
{
    public class SyntaxVisitorFactory : IFactory<INodeVisitor>
    {
        public INodeVisitor Create()
        {
            return new NodeVisitor()
                .Add(typeof(LiteralNode), new LiteralNodeVisitor())
                .Add(typeof(NodeCollection), new NodeCollectionVisitor())
                .Add(typeof(AngleNode), new AngleNodeVisitor())
                .Add(typeof(AngleNameNode), new AngleNameVisitor())
                .Add(typeof(AngleAttributeNode), new AngleAttributeVisitor())
                .Add(typeof(CurlyNode), new CurlyNodeVisitor())
                .Add(typeof(CurlyNameNode), new CurlyNameNodeVisitor())
                .Add(typeof(CurlyDefaultAttributeNode), new CurlyDefaultAttributeNodeVisitor())
                .Add(typeof(CurlyAttributeNode), new CurlyAttributeNodeVisitor());
        }
    }
}
