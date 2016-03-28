using Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Nodes
{
    public class CurlyNode : NodeBase<CurlyNode>
    {
        public Token OpenToken { get; private set; }
        public CurlyNameNode Name { get; private set; }
        public Token CloseToken { get; private set; }
        public IList<CurlyDefaultAttributeNode> DefaultAttributes { get; private set; }
        public IList<CurlyAttributeNode> Attributes { get; private set; }

        public CurlyNode WithOpenToken(Token openToken)
        {
            OpenToken = openToken;
            return this;
        }

        public CurlyNode WithName(CurlyNameNode name)
        {
            Name = name;

            if (name != null)
                name.Parent = this;

            return this;
        }

        public CurlyNode WithCloseToken(Token closeToken)
        {
            CloseToken = closeToken;
            return this;
        }

        public CurlyNode AddDefaultAttribute(CurlyDefaultAttributeNode defaultAttribute)
        {
            Ensure.NotNull(defaultAttribute, "defaultAttribute");
            DefaultAttributes.Add(defaultAttribute);
            defaultAttribute.Parent = this;
            return this;
        }

        public CurlyNode AddAttribute(CurlyAttributeNode attribute)
        {
            Ensure.NotNull(attribute, "attribute");
            Attributes.Add(attribute);
            attribute.Parent = this;
            return this;
        }

        public CurlyNode()
        {
            DefaultAttributes = new List<CurlyDefaultAttributeNode>();
            Attributes = new List<CurlyAttributeNode>();
        }

        protected override CurlyNode CloneInternal()
        {
            CurlyNode result = new CurlyNode
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

    public class CurlyNameNode : NodeBase<CurlyNameNode>
    {
        public Token PrefixToken { get; private set; }
        public Token NameSeparatorToken { get; private set; }
        public Token NameToken { get; private set; }

        public CurlyNameNode WithPrefixToken(Token prefixToken)
        {
            PrefixToken = prefixToken;
            return this;
        }

        public CurlyNameNode WithNameSeparatorToken(Token nameSeparatorToken)
        {
            NameSeparatorToken = nameSeparatorToken;
            return this;
        }

        public CurlyNameNode WithNameToken(Token nameToken)
        {
            NameToken = nameToken;
            return this;
        }

        protected override CurlyNameNode CloneInternal()
        {
            return new CurlyNameNode
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

    public class CurlyDefaultAttributeNode : NodeBase<CurlyDefaultAttributeNode>
    {
        public INode Value { get; private set; }

        public CurlyDefaultAttributeNode WithValue(INode value)
        {
            Value = value;
            if (value != null)
                value.Parent = this;

            return this;
        }

        protected override CurlyDefaultAttributeNode CloneInternal()
        {
            return new CurlyDefaultAttributeNode()
            {
                Value = Value
            };
        }

        protected override IEnumerable<Token> GetTokensInternal()
        {
            return Value.GetTokens();
        }
    }

    public class CurlyAttributeNode : NodeBase<CurlyAttributeNode>
    {
        public Token NameToken { get; private set; }
        public Token ValueSeparatorToken { get; private set; }
        public INode Value { get; private set; }

        public CurlyAttributeNode WithNameToken(Token nameToken)
        {
            NameToken = nameToken;
            return this;
        }

        public CurlyAttributeNode WithValueSeparatorToken(Token valueSeparatorToken)
        {
            ValueSeparatorToken = valueSeparatorToken;
            return this;
        }

        public CurlyAttributeNode WithValue(INode value)
        {
            Value = value;
            if (value != null)
                value.Parent = this;

            return this;
        }

        protected override CurlyAttributeNode CloneInternal()
        {
            CurlyAttributeNode result = new CurlyAttributeNode
            {
                NameToken = NameToken,
                ValueSeparatorToken = ValueSeparatorToken
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
