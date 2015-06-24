using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public abstract class SyntaxNodeBase<T> : ISyntaxNode, ICloneable<T>
        where T : SyntaxNodeBase<T>
    {
        public IList<ComposableToken> LeadingTrivia { get; protected set; }
        public IList<ComposableToken> TrailingTrivia { get; protected set; }

        public SyntaxNodeBase()
        {
            LeadingTrivia = new List<ComposableToken>();
            TrailingTrivia = new List<ComposableToken>();
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

        public IEnumerable<ComposableToken> GetTokens()
        {
            IEnumerable<ComposableToken> result = Enumerable.Empty<ComposableToken>();
            if (LeadingTrivia.Count > 0)
                result = Enumerable.Concat(result, LeadingTrivia);

            IEnumerable<ComposableToken> tokens = GetTokensInternal();
            if (tokens != null)
                result = Enumerable.Concat(result, tokens);

            if (TrailingTrivia.Count > 0)
                result = Enumerable.Concat(result, TrailingTrivia);

            return result;
        }

        protected abstract IEnumerable<ComposableToken> GetTokensInternal();

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (ComposableToken token in GetTokens())
                result.Append(token.Text);

            return result.ToString();
        }
    }
}
