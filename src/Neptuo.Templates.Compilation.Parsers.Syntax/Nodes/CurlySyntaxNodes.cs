using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class CurlySyntax : SyntaxNodeBase<CurlySyntax>
    {
        public Token OpenToken { get; private set; }
        public CurlyNameSyntax Name { get; private set; }
        public Token CloseToken { get; private set; }
        public IList<CurlyDefaultAttributeSyntax> DefaultAttributes { get; private set; }
        public IList<CurlyAttributeSyntax> Attributes { get; private set; }

        public CurlySyntax WithOpenToken(Token openToken)
        {
            OpenToken = openToken;
            return this;
        }

        public CurlySyntax WithName(CurlyNameSyntax name)
        {
            Name = name;

            if (name != null)
                name.Parent = this;

            return this;
        }

        public CurlySyntax WithCloseToken(Token closeToken)
        {
            CloseToken = closeToken;
            return this;
        }

        public CurlySyntax AddDefaultAttribute(CurlyDefaultAttributeSyntax defaultAttribute)
        {
            Ensure.NotNull(defaultAttribute, "defaultAttribute");
            DefaultAttributes.Add(defaultAttribute);
            defaultAttribute.Parent = this;
            return this;
        }

        public CurlySyntax AddAttribute(CurlyAttributeSyntax attribute)
        {
            Ensure.NotNull(attribute, "attribute");
            Attributes.Add(attribute);
            attribute.Parent = this;
            return this;
        }

        public CurlySyntax()
        {
            DefaultAttributes = new List<CurlyDefaultAttributeSyntax>();
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

            if (DefaultAttributes != null && DefaultAttributes.Count > 0)
                result.DefaultAttributes.AddRange(DefaultAttributes.Select(da => da.Clone()));

            if (Attributes != null && Attributes.Count > 0)
                result.Attributes.AddRange(Attributes.Select(a => a.Clone()));

            return result;
        }

        protected override IEnumerable<Token> GetTokensInternal()
        {
            List<Token> result = new List<Token>();

            if (OpenToken != null)
                result.Add(OpenToken);

            if (Name != null)
                result.AddRange(Name.GetTokens());

            if (DefaultAttributes != null && DefaultAttributes.Count > 0)
                result.AddRange(DefaultAttributes.SelectMany(da => da.GetTokens()));

            if (Attributes != null && Attributes.Count > 0)
                result.AddRange(Attributes.SelectMany(a => a.GetTokens()));

            if (CloseToken != null)
                result.Add(CloseToken);

            return result;
        }
    }

    public class CurlyNameSyntax : SyntaxNodeBase<CurlyNameSyntax>
    {
        public Token PrefixToken { get; private set; }
        public Token NameSeparatorToken { get; private set; }
        public Token NameToken { get; private set; }

        public CurlyNameSyntax WithPrefixToken(Token prefixToken)
        {
            PrefixToken = prefixToken;
            return this;
        }

        public CurlyNameSyntax WithNameSeparatorToken(Token nameSeparatorToken)
        {
            NameSeparatorToken = nameSeparatorToken;
            return this;
        }

        public CurlyNameSyntax WithNameToken(Token nameToken)
        {
            NameToken = nameToken;
            return this;
        }

        protected override CurlyNameSyntax CloneInternal()
        {
            return new CurlyNameSyntax
            {
                PrefixToken = PrefixToken,
                NameSeparatorToken = NameSeparatorToken,
                NameToken = NameToken
            };
        }

        protected override IEnumerable<Token> GetTokensInternal()
        {
            List<Token> result = new List<Token>();

            if (PrefixToken != null)
                result.Add(PrefixToken);

            if (NameSeparatorToken != null)
                result.Add(NameSeparatorToken);

            if (NameToken != null)
                result.Add(NameToken);

            return result;
        }
    }

    public class CurlyDefaultAttributeSyntax : SyntaxNodeBase<CurlyDefaultAttributeSyntax>
    {
        public ISyntaxNode Value { get; private set; }

        public CurlyDefaultAttributeSyntax WithValue(ISyntaxNode value)
        {
            Value = value;
            if (value != null)
                value.Parent = this;

            return this;
        }

        protected override CurlyDefaultAttributeSyntax CloneInternal()
        {
            return new CurlyDefaultAttributeSyntax()
            {
                Value = Value
            };
        }

        protected override IEnumerable<Token> GetTokensInternal()
        {
            return Value.GetTokens();
        }
    }

    public class CurlyAttributeSyntax : SyntaxNodeBase<CurlyAttributeSyntax>
    {
        public Token NameToken { get; private set; }
        public Token ValueSeparatorToken { get; private set; }
        public ISyntaxNode Value { get; private set; }

        public CurlyAttributeSyntax WithNameToken(Token nameToken)
        {
            NameToken = nameToken;
            return this;
        }

        public CurlyAttributeSyntax WithValueSeparatorToken(Token valueSeparatorToken)
        {
            ValueSeparatorToken = valueSeparatorToken;
            return this;
        }

        public CurlyAttributeSyntax WithValue(ISyntaxNode value)
        {
            Value = value;
            if (value != null)
                value.Parent = this;

            return this;
        }

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

        protected override IEnumerable<Token> GetTokensInternal()
        {
            List<Token> result = new List<Token>();

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
