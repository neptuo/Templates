﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Templates.Parsers
{
    [TestClass]
    public class Visitor_Mixed : TestVisitorBase
    {
        [TestMethod]
        public void BaseAngleSyntaxProcessor()
        {
            ISyntaxNodeVisitor visitor = CreateVisitor();

            AngleSyntax syntax = new AngleSyntax()
            {
                OpenToken = new TokenFactory().WithText("<"),
                Name = new AngleNameSyntax()
                {
                    NameToken = new TokenFactory().WithText("Literal")
                },
                SelfCloseToken = new TokenFactory().WithText("/"),
                CloseToken = new TokenFactory().WithText(">")
            };

            SyntaxCollection collection = new SyntaxCollection();
            collection.Nodes.Add(syntax);

            Processor1 processor = new Processor1();
            visitor.Visit(collection, processor);

            AssertAreEqual(processor.VisitedTypes.Count, 3);
            AssertAreEqual(processor.VisitedTypes[0], typeof(SyntaxCollection));
            AssertAreEqual(processor.VisitedTypes[1], typeof(AngleSyntax));
            AssertAreEqual(processor.VisitedTypes[2], typeof(AngleNameSyntax));
        }

        private class Processor1 : ISyntaxNodeProcessor
        {
            public readonly List<Type> VisitedTypes = new List<Type>();

            public void Process(ISyntaxNode node)
            {
                Assert.IsNotNull(node);
                VisitedTypes.Add(node.GetType());
            }
        }

    }
}
