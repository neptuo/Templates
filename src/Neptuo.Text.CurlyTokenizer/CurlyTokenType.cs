using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Syntax.Tokenizers
{
    public class CurlyTokenType : TokenType
    {
        public static readonly TokenType OpenBrace = new TokenType("Curly.OpenBrace");
        public static readonly TokenType NamePrefix = new TokenType("Curly.NamePrefix");
        public static readonly TokenType NameSeparator = new TokenType("Curly.NameSeparator");
        public static readonly TokenType Name = new TokenType("Curly.Name");

        public static readonly TokenType DefaultAttributeValue = new TokenType("Curly.DefaultAttributeName");
        public static readonly TokenType AttributeSeparator = new TokenType("Curly.AttributeSeparator");

        public static readonly TokenType AttributeName = new TokenType("Curly.AttributeName");
        public static readonly TokenType AttributeValueSeparator = new TokenType("Curly.AttributeValueSeparator");

        public static readonly TokenType CloseBrace = new TokenType("Curly.CloseBrace");

        private CurlyTokenType() 
            : base("")
        { }
    }
}
