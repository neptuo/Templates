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
        public Token OpenToken { get; set; }
        public AngleNameSyntax Name { get; set; }
        public Token SelfCloseToken { get; set; }
        public Token CloseToken { get; set; }
        public IList<AngleAttributeSyntax> Attributes { get; set; }

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
        public Token PrefixToken { get; set; }
        public Token NameSeparatorToken { get; set; }
        public Token NameToken { get; set; }

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
        public AngleNameSyntax Name { get; set; }
        public Token ValueSeparatorToken { get; set; }
        public Token AttributeOpenValueToken { get; set; }
        public ISyntaxNode Value { get; set; }
        public Token AttributeCloseValueToken { get; set; }

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
