using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers
{
    public abstract class TestVisitorBase : TestBase
    {
        protected ISyntaxNodeVisitor CreateVisitor()
        {
            return new SyntaxVisitor()
                .Add(typeof(LiteralSyntax), new LiteralSyntaxVisitor())
                .Add(typeof(SyntaxCollection), new SyntaxCollectionVisitor())
                .Add(typeof(AngleSyntax), new AngleSyntaxVisitor())
                .Add(typeof(AngleNameSyntax), new AngleNameSyntaxVisitor())
                .Add(typeof(AngleAttributeSyntax), new AngleAttributeSyntaxVisitor())
                .Add(typeof(CurlySyntax), new CurlySyntaxVisitor())
                .Add(typeof(CurlyNameSyntax), new CurlyNameSyntaxVisitor())
                .Add(typeof(CurlyAttributeSyntax), new CurlyAttributeSyntaxVisitor());
        }
    }
}
