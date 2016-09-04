﻿using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes.Visitors
{
    public class TokenAtPositionFinder : INodeProcessor
    {
        public int Position { get; private set; }
        public INode BestMatch { get; private set; }
        public bool IsVirtualIncluded { get; private set; }

        public TokenAtPositionFinder(int position, bool isVirtualIncluded = false)
        {
            Ensure.PositiveOrZero(position, "position");
            Position = position;
            IsVirtualIncluded = isVirtualIncluded;
        }

        public bool Process(INode node)
        {
            bool isMatch = node.GetTokens().Any(IsTokenMatch);
            if (isMatch)
                BestMatch = node;

            return true;
        }

        private bool IsTokenMatch(Token token)
        {
            if (!IsVirtualIncluded && (token.IsSkipped || token.IsVirtual))
                return false;

            return token.TextSpan.StartIndex <= Position && token.TextSpan.StartIndex + token.TextSpan.Length >= Position;
        }
    }
}
