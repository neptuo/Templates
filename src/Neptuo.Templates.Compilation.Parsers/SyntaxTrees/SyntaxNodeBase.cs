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
        private readonly List<ComposableToken> tokens = new List<ComposableToken>();
        private List<ComposableToken> leadingTrivia;
        private List<ComposableToken> trailingTrivia;

        public IReadOnlyList<ComposableToken> Tokens
        {
            get { return tokens; }
        }

        public IReadOnlyList<ComposableToken> LeadingTrivia
        {
            get { return leadingTrivia; }
        }

        public IReadOnlyList<ComposableToken> TrailingTrivia
        {
            get { return trailingTrivia; }
        }

        public SyntaxNodeBase(IEnumerable<ComposableToken> leadingTrivia, IEnumerable<ComposableToken> trailingTrivia)
        {
            if (leadingTrivia != null)
                this.leadingTrivia = new List<ComposableToken>(leadingTrivia);

            if (trailingTrivia != null)
                this.trailingTrivia = new List<ComposableToken>(trailingTrivia);
        }

        #region Total tokens

        protected void AddNullableTokens(params ISyntaxNode[] nodes)
        {
            AddNullableTokens((IEnumerable<ISyntaxNode>)nodes);
        }

        protected void AddNullableTokens(IEnumerable<ISyntaxNode> nodes)
        {
            if (nodes == null)
                return;

            foreach (ISyntaxNode node in nodes)
            {
                if (node != null)
                    AddNullableTokens(node.Tokens);
            }
        }

        protected void AddNullableTokens(params ComposableToken[] tokens)
        {
            AddNullableTokens((IEnumerable<ComposableToken>)tokens);
        }

        protected void AddNullableTokens(IEnumerable<ComposableToken> tokens)
        {
            foreach (ComposableToken token in tokens)
            {
                if (token != null)
                    this.tokens.Add(token);
            }
        }

        #endregion

        #region Trivia

        protected void AddNullableLeadingTrivia()
        {
            if (leadingTrivia != null)
                AddNullableTokens(leadingTrivia);
        }

        protected void AddNullableTrailingTrivia()
        {
            if (trailingTrivia != null)
                AddNullableTokens(trailingTrivia);
        }

        private void AddNullableLeadingTrivia(params ComposableToken[] tokens)
        {
            AddNullableLeadingTrivia((IEnumerable<ComposableToken>)tokens);
        }

        private void AddNullableLeadingTrivia(IEnumerable<ComposableToken> tokens)
        {
            if (tokens == null)
                return;

            if (leadingTrivia == null)
                leadingTrivia = new List<ComposableToken>();

            foreach (ComposableToken token in tokens)
            {
                if (token != null)
                {
                    this.leadingTrivia.Add(token);
                    this.tokens.Add(token);
                }
            }
        }

        private void AddNullableTrailingTrivia(params ComposableToken[] tokens)
        {
            AddNullableTrailingTrivia((IEnumerable<ComposableToken>)tokens);
        }

        private void AddNullableTrailingTrivia(IEnumerable<ComposableToken> tokens)
        {
            if (tokens == null)
                return;

            if (trailingTrivia == null)
                trailingTrivia = new List<ComposableToken>();

            foreach (ComposableToken token in tokens)
            {
                if (token != null)
                {
                    this.trailingTrivia.Add(token);
                    this.tokens.Add(token);
                }
            }
        }

        #endregion

        public T WithLeadingTrivia(params ComposableToken[] leadingTrivia)
        {
            return WithLeadingTrivia((IEnumerable<ComposableToken>)leadingTrivia);
        }

        public T WithLeadingTrivia(IEnumerable<ComposableToken> leadingTrivia)
        {
            SyntaxNodeBase<T> result = (SyntaxNodeBase<T>)Clone();
            result.AddNullableLeadingTrivia(leadingTrivia);
            return (T)result;
        }

        public T AddLeadingTrivia(params ComposableToken[] leadingTrivia)
        {
            if (leadingTrivia == null)
                return Clone();

            if (this.leadingTrivia == null)
                return WithLeadingTrivia(leadingTrivia);
            
            return WithLeadingTrivia(Enumerable.Concat(this.leadingTrivia, leadingTrivia));
        }

        public T WithTrailingTrivia(params ComposableToken[] trailingTrivia)
        {
            return WithTrailingTrivia((IEnumerable<ComposableToken>)trailingTrivia);
        }

        public T WithTrailingTrivia(IEnumerable<ComposableToken> trailingTrivia)
        {
            SyntaxNodeBase<T> result = (SyntaxNodeBase<T>)Clone();
            result.AddNullableTrailingTrivia(trailingTrivia);
            return (T)result;
        }

        public T AddTrailingTrivia(params ComposableToken[] trailingTrivia)
        {
            if (trailingTrivia == null)
                return Clone();

            if (this.trailingTrivia == null)
                return WithTrailingTrivia(trailingTrivia);

            return WithTrailingTrivia(Enumerable.Concat(this.trailingTrivia, trailingTrivia));
        }

        public abstract T Clone();
    }
}
