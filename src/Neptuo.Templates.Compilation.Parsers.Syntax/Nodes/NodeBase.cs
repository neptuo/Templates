using Neptuo.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public abstract class NodeBase<T> : INode, ICloneable<T>
        where T : NodeBase<T>
    {
        public INode Parent { get; set; }
        public IList<Token> LeadingTrivia { get; protected set; }
        public IList<Token> TrailingTrivia { get; protected set; }

        public NodeBase()
        {
            LeadingTrivia = new List<Token>();
            TrailingTrivia = new List<Token>();
        }

        public T Clone()
        {
            T clone = CloneInternal();

            if (LeadingTrivia.Count > 0)
                clone.LeadingTrivia.AddRange(LeadingTrivia);

            if (TrailingTrivia.Count > 0)
                clone.TrailingTrivia.AddRange(TrailingTrivia);

            return clone;
        }

        protected abstract T CloneInternal();

        public IEnumerable<Token> GetTokens()
        {
            IEnumerable<Token> result = Enumerable.Empty<Token>();
            if (LeadingTrivia.Count > 0)
                result = Enumerable.Concat(result, LeadingTrivia);

            IEnumerable<Token> tokens = GetTokensInternal();
            if (tokens != null)
                result = Enumerable.Concat(result, tokens);

            if (TrailingTrivia.Count > 0)
                result = Enumerable.Concat(result, TrailingTrivia);

            return result;
        }

        protected abstract IEnumerable<Token> GetTokensInternal();

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (Token token in GetTokens())
                result.Append(token.Text);

            return result.ToString();
        }
    }
}
