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
    public class SyntaxVisitorFactory : IFactory<ISyntaxNodeVisitor>
    {
        public ISyntaxNodeVisitor Create()
        {
            return new SyntaxVisitor()
                .Add(typeof(LiteralSyntax), new LiteralSyntaxVisitor())
                .Add(typeof(SyntaxCollection), new SyntaxCollectionVisitor())
                .Add(typeof(AngleSyntax), new AngleSyntaxVisitor())
                .Add(typeof(AngleNameSyntax), new AngleNameSyntaxVisitor())
                .Add(typeof(AngleAttributeSyntax), new AngleAttributeSyntaxVisitor())
                .Add(typeof(CurlySyntax), new CurlySyntaxVisitor())
                .Add(typeof(CurlyNameSyntax), new CurlyNameSyntaxVisitor())
                .Add(typeof(CurlyDefaultAttributeSyntax), new CurlyDefaultAttributeSyntaxVisitor())
                .Add(typeof(CurlyAttributeSyntax), new CurlyAttributeSyntaxVisitor());
        }
    }
}
