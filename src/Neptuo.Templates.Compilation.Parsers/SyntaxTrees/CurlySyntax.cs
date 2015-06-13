using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class CurlySyntax : SyntaxNodeBase<CurlySyntax>
    {
        public ComposableToken OpenToken { get; private set; }
        public CurlyNameSyntax Name { get; private set; }
        public ComposableToken CloseToken { get; private set; }
        public IReadOnlyList<CurlyAttributeSyntax> Attributes { get; private set; }

        internal CurlySyntax(ComposableToken openToken, CurlyNameSyntax name, ComposableToken closeToken, IReadOnlyList<CurlyAttributeSyntax> attributes, IEnumerable<ComposableToken> leadingTrivia, IEnumerable<ComposableToken> trailingTrivia)
            : base(leadingTrivia, trailingTrivia)
        {
            OpenToken = openToken;
            Name = name;
            CloseToken = closeToken;
            Attributes = attributes;

            AddNullableLeadingTrivia();
            AddNullableTokens(openToken);
            AddNullableTokens(name);
            AddNullableTokens(attributes);
            AddNullableTokens(closeToken);
            AddNullableTrailingTrivia();
        }

        public override CurlySyntax Clone()
        {
            return new CurlySyntax(OpenToken, Name, CloseToken, Attributes, LeadingTrivia, TrailingTrivia);
        }

        public CurlySyntax WithOpenToken(ComposableToken openToken)
        {
            return new CurlySyntax(openToken, Name, CloseToken, Attributes, LeadingTrivia, TrailingTrivia);
        }

        public CurlySyntax WithName(CurlyNameSyntax name)
        {
            return new CurlySyntax(OpenToken, name, CloseToken, Attributes, LeadingTrivia, TrailingTrivia);
        }

        public CurlySyntax WithCloseToken(ComposableToken closeToken)
        {
            return new CurlySyntax(OpenToken, Name, closeToken, Attributes, LeadingTrivia, TrailingTrivia);
        }

        public CurlySyntax AddAttribute(CurlyAttributeSyntax attribute)
        {
            Ensure.NotNull(attribute, "attribute");

            List<CurlyAttributeSyntax> attributes = new List<CurlyAttributeSyntax>();
            if(Attributes != null)
                attributes.AddRange(Attributes);

            attributes.Add(attribute);
            return new CurlySyntax(OpenToken, Name, CloseToken, attributes, LeadingTrivia, TrailingTrivia);
        }


        public static CurlySyntax New()
        {
            return new CurlySyntax(null, null, null, null, null, null);
        }
    }

    public class CurlyNameSyntax : SyntaxNodeBase<CurlyNameSyntax>
    {
        public ComposableToken PrefixToken { get; private set; }
        public ComposableToken NameSeparatorToken { get; private set; }
        public ComposableToken NameToken { get; private set; }

        internal CurlyNameSyntax(ComposableToken prefixToken, ComposableToken nameSeparatorToken, ComposableToken nameToken, IEnumerable<ComposableToken> leadingTrivia, IEnumerable<ComposableToken> trailingTrivia)
            : base(leadingTrivia, trailingTrivia)
        {
            PrefixToken = prefixToken;
            NameSeparatorToken = nameSeparatorToken;
            NameToken = nameToken;

            AddNullableLeadingTrivia();
            AddNullableTokens(prefixToken, nameSeparatorToken, nameToken);
            AddNullableTrailingTrivia();
        }

        public override CurlyNameSyntax Clone()
        {
            return new CurlyNameSyntax(PrefixToken, NameSeparatorToken, NameToken, LeadingTrivia, TrailingTrivia);
        }

        public CurlyNameSyntax WithPrefixToken(ComposableToken prefixToken)
        {
            return new CurlyNameSyntax(prefixToken, NameSeparatorToken, NameToken, LeadingTrivia, TrailingTrivia);
        }

        public CurlyNameSyntax WithNameSeparatorToken(ComposableToken nameSeparatorToken)
        {
            return new CurlyNameSyntax(PrefixToken, nameSeparatorToken, NameToken, LeadingTrivia, TrailingTrivia);
        }

        public CurlyNameSyntax WithNameToken(ComposableToken nameToken)
        {
            return new CurlyNameSyntax(PrefixToken, NameSeparatorToken, nameToken, LeadingTrivia, TrailingTrivia);
        }


        public static CurlyNameSyntax New()
        {
            return new CurlyNameSyntax(null, null, null, null, null);
        }
    }

    public class CurlyAttributeSyntax : SyntaxNodeBase<CurlyAttributeSyntax>
    {
        public ComposableToken NameToken { get; private set; }
        public ComposableToken ValueSeparatorToken { get; private set; }
        public ISyntaxNode Value { get; private set; }

        public CurlyAttributeSyntax(ComposableToken nameToken, ComposableToken valueSeparatorToken, ISyntaxNode value, IEnumerable<ComposableToken> leadingTrivia, IEnumerable<ComposableToken> trailingTrivia)
            : base(leadingTrivia, trailingTrivia)
        {
            NameToken = nameToken;
            ValueSeparatorToken = valueSeparatorToken;
            Value = value;

            AddNullableLeadingTrivia();
            AddNullableTokens(NameToken, valueSeparatorToken);
            AddNullableTokens(value);
            AddNullableTrailingTrivia();
        }

        public override CurlyAttributeSyntax Clone()
        {
            return new CurlyAttributeSyntax(NameToken, ValueSeparatorToken, Value, LeadingTrivia, TrailingTrivia);
        }

        public CurlyAttributeSyntax WithNameToken(ComposableToken nameToken)
        {
            return new CurlyAttributeSyntax(nameToken, ValueSeparatorToken, Value, LeadingTrivia, TrailingTrivia);
        }

        public CurlyAttributeSyntax WithValueSeparatorToken(ComposableToken valueSeparatorToken)
        {
            return new CurlyAttributeSyntax(NameToken, valueSeparatorToken, Value, LeadingTrivia, TrailingTrivia);
        }

        public CurlyAttributeSyntax WithValueToken(ISyntaxNode value)
        {
            return new CurlyAttributeSyntax(NameToken, ValueSeparatorToken, value, LeadingTrivia, TrailingTrivia);
        }


        public static CurlyAttributeSyntax New()
        {
            return new CurlyAttributeSyntax(null, null, null, null, null);
        }
    }

}
