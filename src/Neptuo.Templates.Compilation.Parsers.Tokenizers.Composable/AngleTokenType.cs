using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class AngleTokenType : TokenType
    {
        public static readonly TokenType OpenBrace = new TokenType("Angle.OpenBrace");
        public static readonly TokenType NamePrefix = new TokenType("Angle.NamePrefix");
        public static readonly TokenType NameSeparator = new TokenType("Angle.NameSeparator");
        public static readonly TokenType Name = new TokenType("Angle.Name");
        public static readonly TokenType SelfCloseBrace = new TokenType("Angle.SelfCloseBrace");
        public static readonly TokenType CloseBrace = new TokenType("Angle.CloseBrace");

        public static readonly TokenType AttributeNamePrefix = new TokenType("Angle.AttributeNamePrefix");
        public static readonly TokenType AttributeNameSeparator = new TokenType("Angle.AttributeNameSeparator");
        public static readonly TokenType AttributeName = new TokenType("Angle.AttributeName");
        public static readonly TokenType AttributeValueSeparator = new TokenType("Angle.AttributeValueSeparator");
        public static readonly TokenType AttributeOpenValue = new TokenType("Angle.AttributeOpenValue");
        public static readonly TokenType AttributeCloseValue = new TokenType("Angle.AttributeCloseValue");

        public static readonly TokenType OpenDirective = new TokenType("Angle.OpenDirective");

        public static readonly TokenType OpenComment = new TokenType("Angle.OpenComment");
        public static readonly TokenType CloseComment = new TokenType("Angle.CloseComment");

        public static readonly TokenType Error = new TokenType("Error");

        private AngleTokenType() 
            : base("")
        { }
    }
}
