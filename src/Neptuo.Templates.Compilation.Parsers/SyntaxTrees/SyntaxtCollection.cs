﻿using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class SyntaxtCollection : ISyntaxNode
    {
        public IList<ISyntaxNode> Nodes { get; private set; }

        public IReadOnlyList<ComposableToken> Tokens
        {
            get { return Nodes.SelectMany(n => n.Tokens).ToList(); }
        }
        public IReadOnlyList<ComposableToken> LeadingTrivia
        {
            get { return null; }
        }

        public IReadOnlyList<ComposableToken> TrailingTrivia
        {
            get { return null; }
        }

        public SyntaxtCollection()
        {
            Nodes = new List<ISyntaxNode>();
        }
    }
}
