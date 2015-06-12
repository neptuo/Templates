using Neptuo.Templates.Compilation.Parsers.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.SyntaxTrees
{
    public class CurlySyntax : SyntaxNodeBase
    {
        public ComposableToken OpenToken { get; private set; }
        public CurlyNameSyntax Name { get; private set; }
        public ComposableToken CloseToken { get; private set; }
        public IReadOnlyList<CurlyAttributeSyntax> Attributes { get; private set; }

        internal CurlySyntax(ComposableToken openToken, CurlyNameSyntax name, ComposableToken closeToken, IReadOnlyList<CurlyAttributeSyntax> attributes)
        {
            OpenToken = openToken;
            Name = name;
            CloseToken = closeToken;
            Attributes = attributes;

            AddNullableTokens(openToken);
            AddNullableTokens(name);
            AddNullableTokens(attributes);
            AddNullableTokens(closeToken);
        }

        public CurlySyntax WithOpenToken(ComposableToken openToken)
        {
            return new CurlySyntax(openToken, Name, CloseToken, Attributes);
        }

        public CurlySyntax WithName(CurlyNameSyntax name)
        {
            return new CurlySyntax(OpenToken, name, CloseToken, Attributes);
        }

        public CurlySyntax WithCloseToken(ComposableToken closeToken)
        {
            return new CurlySyntax(OpenToken, Name, closeToken, Attributes);
        }

        public CurlySyntax AddAttribute(CurlyAttributeSyntax attribute)
        {
            Ensure.NotNull(attribute, "attribute");

            List<CurlyAttributeSyntax> attributes = new List<CurlyAttributeSyntax>();
            if(Attributes != null)
                attributes.AddRange(Attributes);

            attributes.Add(attribute);
            return new CurlySyntax(OpenToken, Name, CloseToken, attributes);
        }


        public static CurlySyntax New()
        {
            return new CurlySyntax(null, null, null, null);
        }
    }

    public class CurlyNameSyntax : SyntaxNodeBase
    {
        public ComposableToken PrefixToken { get; private set; }
        public ComposableToken NameSeparatorToken { get; private set; }
        public ComposableToken NameToken { get; private set; }

        internal CurlyNameSyntax(ComposableToken prefixToken, ComposableToken nameSeparatorToken, ComposableToken nameToken)
        {
            PrefixToken = prefixToken;
            NameSeparatorToken = nameSeparatorToken;
            NameToken = nameToken;

            AddNullableTokens(prefixToken, nameSeparatorToken, nameToken);
        }

        public CurlyNameSyntax WithPrefixToken(ComposableToken prefixToken)
        {
            return new CurlyNameSyntax(prefixToken, NameSeparatorToken, NameToken);
        }

        public CurlyNameSyntax WithNameSeparatorToken(ComposableToken nameSeparatorToken)
        {
            return new CurlyNameSyntax(PrefixToken, nameSeparatorToken, NameToken);
        }

        public CurlyNameSyntax WithNameToken(ComposableToken nameToken)
        {
            return new CurlyNameSyntax(PrefixToken, NameSeparatorToken, nameToken);
        }


        public static CurlyNameSyntax New()
        {
            return new CurlyNameSyntax(null, null, null);
        }
    }

    public class CurlyAttributeSyntax : SyntaxNodeBase
    {
        public ComposableToken NameToken { get; private set; }
        public ComposableToken ValueSeparatorToken { get; private set; }
        public ComposableToken ValueToken { get; private set; }

        public CurlyAttributeSyntax(ComposableToken nameToken, ComposableToken valueSeparatorToken, ComposableToken valueToken)
        {
            NameToken = nameToken;
            ValueSeparatorToken = valueSeparatorToken;
            ValueToken = valueToken;
            AddNullableTokens(NameToken, valueSeparatorToken, valueToken);
        }

        public CurlyAttributeSyntax WithNameToken(ComposableToken nameToken)
        {
            return new CurlyAttributeSyntax(nameToken, ValueSeparatorToken, ValueToken);
        }

        public CurlyAttributeSyntax WithValueSeparatorToken(ComposableToken valueSeparatorToken)
        {
            return new CurlyAttributeSyntax(NameToken, valueSeparatorToken, ValueToken);
        }

        public CurlyAttributeSyntax WithValueToken(ComposableToken valueToken)
        {
            return new CurlyAttributeSyntax(NameToken, ValueSeparatorToken, valueToken);
        }


        public static CurlyAttributeSyntax New()
        {
            return new CurlyAttributeSyntax(null, null, null);
        }
    }

}
