﻿using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class CurlySyntax : SyntaxNodeBase<CurlySyntax>
    {
        public ComposableToken OpenToken { get; set; }
        public CurlyNameSyntax Name { get; set; }
        public ComposableToken CloseToken { get; set; }
        public IList<CurlyAttributeSyntax> Attributes { get; set; }

        public CurlySyntax()
        {
            Attributes = new List<CurlyAttributeSyntax>();
        }

        protected override CurlySyntax CloneInternal()
        {
            CurlySyntax result = new CurlySyntax
            {
                OpenToken = OpenToken,
                Name = Name != null ? Name.Clone() : null,
                CloseToken = CloseToken
            };

            if (Attributes.Count > 0)
                result.Attributes.AddRange(Attributes.Select(a => a.Clone()));

            return result;
        }

        protected override IEnumerable<ComposableToken> GetTokensInternal()
        {
            List<ComposableToken> result = new List<ComposableToken>();

            if (OpenToken != null)
                result.Add(OpenToken);

            if (Name != null)
                result.AddRange(Name.GetTokens());

            if (Attributes.Count > 0)
                result.AddRange(Attributes.SelectMany(a => a.GetTokens()));

            if (CloseToken != null)
                result.Add(CloseToken);

            return result;
        }
    }

    public class CurlyNameSyntax : SyntaxNodeBase<CurlyNameSyntax>
    {
        public ComposableToken PrefixToken { get; set; }
        public ComposableToken NameSeparatorToken { get; set; }
        public ComposableToken NameToken { get; set; }

        protected override CurlyNameSyntax CloneInternal()
        {
            return new CurlyNameSyntax
            {
                PrefixToken = PrefixToken,
                NameSeparatorToken = NameSeparatorToken,
                NameToken = NameToken
            };
        }

        protected override IEnumerable<ComposableToken> GetTokensInternal()
        {
            List<ComposableToken> result = new List<ComposableToken>();

            if (PrefixToken != null)
                result.Add(PrefixToken);

            if (NameSeparatorToken != null)
                result.Add(NameSeparatorToken);

            if (NameToken != null)
                result.Add(NameToken);

            return result;
        }
    }

    public class CurlyAttributeSyntax : SyntaxNodeBase<CurlyAttributeSyntax>
    {
        public ComposableToken NameToken { get; set; }
        public ComposableToken ValueSeparatorToken { get; set; }
        public ISyntaxNode Value { get; set; }

        protected override CurlyAttributeSyntax CloneInternal()
        {
            CurlyAttributeSyntax result = new CurlyAttributeSyntax
            {
                NameToken = NameToken,
                ValueSeparatorToken = ValueSeparatorToken
            };

            ICloneable<ISyntaxNode> cloneableValue = Value as ICloneable<ISyntaxNode>;
            if (cloneableValue != null)
                result.Value = cloneableValue.Clone();
            else if (Value != null) 
                throw Ensure.Condition.NotCloneable(Value);

            return result;
        }

        protected override IEnumerable<ComposableToken> GetTokensInternal()
        {
            List<ComposableToken> result = new List<ComposableToken>();

            if (NameToken != null)
                result.Add(NameToken);

            if (ValueSeparatorToken != null)
                result.Add(ValueSeparatorToken);

            if (Value != null)
                result.AddRange(Value.GetTokens());

            return result;
        }
    }
}