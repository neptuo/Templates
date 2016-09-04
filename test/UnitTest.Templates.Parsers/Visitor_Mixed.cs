﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes;
using Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors;
using Neptuo.Text;
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
            INodeVisitor visitor = CreateVisitor();

            AngleNode syntax = new AngleNode()
                .WithOpenToken(new TokenFactory().WithText("<"))
                .WithName(
                    new AngleNameNode()
                        .WithNameToken(new TokenFactory().WithText("Literal"))
                )
                .WithSelfCloseToken(new TokenFactory().WithText("/"))
                .WithCloseToken(new TokenFactory().WithText(">"));

            NodeCollection collection = new NodeCollection();
            collection.Add(syntax);

            Processor1 processor = new Processor1();
            visitor.Visit(collection, processor);

            AssertAreEqual(processor.VisitedTypes.Count, 3);
            AssertAreEqual(processor.VisitedTypes[0], typeof(NodeCollection));
            AssertAreEqual(processor.VisitedTypes[1], typeof(AngleNode));
            AssertAreEqual(processor.VisitedTypes[2], typeof(AngleNameNode));
        }

        private class Processor1 : INodeProcessor
        {
            public readonly List<Type> VisitedTypes = new List<Type>();

            public bool Process(INode node)
            {
                Assert.IsNotNull(node);

                if (VisitedTypes.Count != 0)
                    Assert.IsNotNull(node.Parent);

                VisitedTypes.Add(node.GetType());
                return true;
            }
        }

    }
}
