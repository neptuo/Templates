using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class AngleNode : NodeBase<AngleNode>
    {
        public Token OpenToken { get; private set; }
        public AngleNameNode Name { get; private set; }
        public Token SelfCloseToken { get; private set; }
        public Token CloseToken { get; private set; }
        public IList<AngleAttributeNode> Attributes { get; private set; }

        public AngleNode WithOpenToken(Token openToken)
        {
            OpenToken = openToken;
            return this;
        }

        public AngleNode WithName(AngleNameNode name)
        {
            Name = name;
            if (name != null) 
                name.Parent = this;

            return this;
        }

        public AngleNode WithSelfCloseToken(Token selfCloseToken)
        {
            SelfCloseToken = selfCloseToken;
            return this;
        }

        public AngleNode WithCloseToken(Token closeToken)
        {
            CloseToken = closeToken;
            return this;
        }

        public AngleNode AddAttribute(AngleAttributeNode attribute)
        {
            Ensure.NotNull(attribute, "attribute");
            Attributes.Add(attribute);
            attribute.Parent = this;
            return this;
        }

        public AngleNode()
        {
            Attributes = new List<AngleAttributeNode>();
        }

        protected override AngleNode CloneInternal()
        {
            AngleNode result = new AngleNode
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

    public class AngleNameNode : NodeBase<AngleNameNode>
    {
        public Token PrefixToken { get; private set; }
        public Token NameSeparatorToken { get; private set; }
        public Token NameToken { get; private set; }

        public AngleNameNode WithPrefixToken(Token prefixToken)
        {
            PrefixToken = prefixToken;
            return this;
        }

        public AngleNameNode WithNameSeparatorToken(Token nameSeparatorToken)
        {
            NameSeparatorToken = nameSeparatorToken;
            return this;
        }

        public AngleNameNode WithNameToken(Token nameToken)
        {
            NameToken = nameToken;
            return this;
        }

        protected override AngleNameNode CloneInternal()
        {
            return new AngleNameNode
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

    public class AngleAttributeNode : NodeBase<AngleAttributeNode>
    {
        public AngleNameNode Name { get; private set; }
        public Token ValueSeparatorToken { get; private set; }
        public Token AttributeOpenValueToken { get; private set; }
        public INode Value { get; private set; }
        public Token AttributeCloseValueToken { get; private set; }

        public AngleAttributeNode WithName(AngleNameNode name)
        {
            Name = name;
            if (name != null)
                name.Parent = this;

            return this;
        }

        public AngleAttributeNode WithValueSeparatorToken(Token valueSeparatorToken)
        {
            ValueSeparatorToken = valueSeparatorToken;
            return this;
        }

        public AngleAttributeNode WithAttributeOpenValueToken(Token attributeOpenValueToken)
        {
            AttributeOpenValueToken = attributeOpenValueToken;
            return this;
        }

        public AngleAttributeNode WithValue(INode value)
        {
            Value = value;
            if (value != null)
                value.Parent = this;

            return this;
        }

        public AngleAttributeNode WithAttributeCloseValueToken(Token attributeCloseValueToken)
        {
            AttributeCloseValueToken = attributeCloseValueToken;
            return this;
        }

        protected override AngleAttributeNode CloneInternal()
        {
            AngleAttributeNode result = new AngleAttributeNode
            {
                Name = Name.Clone(),
                ValueSeparatorToken = ValueSeparatorToken,
                AttributeOpenValueToken = AttributeOpenValueToken,
                AttributeCloseValueToken = AttributeCloseValueToken
            };

            ICloneable<INode> cloneableValue = Value as ICloneable<INode>;
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
