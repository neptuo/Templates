using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class AngleSyntax : SyntaxNodeBase<AngleSyntax>
    {
        public Token OpenToken { get; private set; }
        public AngleNameSyntax Name { get; private set; }
        public Token SelfCloseToken { get; private set; }
        public Token CloseToken { get; private set; }
        public IList<AngleAttributeSyntax> Attributes { get; private set; }

        public AngleSyntax WithOpenToken(Token openToken)
        {
            OpenToken = openToken;
            return this;
        }

        public AngleSyntax WithName(AngleNameSyntax name)
        {
            Name = name;
            if (name != null) 
                name.Parent = this;

            return this;
        }

        public AngleSyntax WithSelfCloseToken(Token selfCloseToken)
        {
            SelfCloseToken = selfCloseToken;
            return this;
        }

        public AngleSyntax WithCloseToken(Token closeToken)
        {
            CloseToken = closeToken;
            return this;
        }

        public AngleSyntax AddAttribute(AngleAttributeSyntax attribute)
        {
            Ensure.NotNull(attribute, "attribute");
            Attributes.Add(attribute);
            attribute.Parent = this;
            return this;
        }

        public AngleSyntax()
        {
            Attributes = new List<AngleAttributeSyntax>();
        }

        protected override AngleSyntax CloneInternal()
        {
            AngleSyntax result = new AngleSyntax
            {
                OpenToken = OpenToken,
                Name = Name != null ? Name.Clone() : null,
                SelfCloseToken = SelfCloseToken,
                CloseToken = CloseToken
            };

            if (Attributes.Count > 0)
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

            if (Attributes.Count > 0)
                result.AddRange(Attributes.SelectMany(a => a.GetTokens()));

            if (SelfCloseToken != null)
                result.Add(SelfCloseToken);

            if (CloseToken != null)
                result.Add(CloseToken);

            return result;
        }
    }

    public class AngleNameSyntax : SyntaxNodeBase<AngleNameSyntax>
    {
        public Token PrefixToken { get; private set; }
        public Token NameSeparatorToken { get; private set; }
        public Token NameToken { get; private set; }

        public AngleNameSyntax WithPrefixToken(Token prefixToken)
        {
            PrefixToken = prefixToken;
            return this;
        }

        public AngleNameSyntax WithNameSeparatorToken(Token nameSeparatorToken)
        {
            NameSeparatorToken = nameSeparatorToken;
            return this;
        }

        public AngleNameSyntax WithNameToken(Token nameToken)
        {
            NameToken = nameToken;
            return this;
        }

        protected override AngleNameSyntax CloneInternal()
        {
            return new AngleNameSyntax
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

    public class AngleAttributeSyntax : SyntaxNodeBase<AngleAttributeSyntax>
    {
        public AngleNameSyntax Name { get; private set; }
        public Token ValueSeparatorToken { get; private set; }
        public Token AttributeOpenValueToken { get; private set; }
        public ISyntaxNode Value { get; private set; }
        public Token AttributeCloseValueToken { get; private set; }

        public AngleAttributeSyntax WithName(AngleNameSyntax name)
        {
            Name = name;
            if (name != null)
                name.Parent = this;

            return this;
        }

        public AngleAttributeSyntax WithValueSeparatorToken(Token valueSeparatorToken)
        {
            ValueSeparatorToken = valueSeparatorToken;
            return this;
        }

        public AngleAttributeSyntax WithAttributeOpenValueToken(Token attributeOpenValueToken)
        {
            AttributeOpenValueToken = attributeOpenValueToken;
            return this;
        }

        public AngleAttributeSyntax WithValue(ISyntaxNode value)
        {
            Value = value;
            if (value != null)
                value.Parent = this;

            return this;
        }

        public AngleAttributeSyntax WithAttributeCloseValueToken(Token attributeCloseValueToken)
        {
            AttributeCloseValueToken = attributeCloseValueToken;
            return this;
        }

        protected override AngleAttributeSyntax CloneInternal()
        {
            AngleAttributeSyntax result = new AngleAttributeSyntax
            {
                Name = Name.Clone(),
                ValueSeparatorToken = ValueSeparatorToken,
                AttributeOpenValueToken = AttributeOpenValueToken,
                AttributeCloseValueToken = AttributeCloseValueToken
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

            if (Name != null)
                result.AddRange(Name.GetTokens());

            if (ValueSeparatorToken != null)
                result.Add(ValueSeparatorToken);

            if (AttributeOpenValueToken != null)
                result.Add(AttributeOpenValueToken);

            if (Value != null)
                result.AddRange(Value.GetTokens());

            if (AttributeCloseValueToken != null)
                result.Add(AttributeCloseValueToken);

            return result;
        }
    }
}
