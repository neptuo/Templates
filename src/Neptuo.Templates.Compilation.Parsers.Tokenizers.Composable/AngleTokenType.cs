using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class AngleTokenType : ComposableTokenType
    {
        public static readonly ComposableTokenType OpenBrace = new ComposableTokenType("Angle.OpenBrace");
        public static readonly ComposableTokenType NamePrefix = new ComposableTokenType("Angle.NamePrefix");
        public static readonly ComposableTokenType NameSeparator = new ComposableTokenType("Angle.NameSeparator");
        public static readonly ComposableTokenType Name = new ComposableTokenType("Angle.Name");
        public static readonly ComposableTokenType SelfCloseBrace = new ComposableTokenType("Angle.SelfCloseBrace");
        public static readonly ComposableTokenType CloseBrace = new ComposableTokenType("Angle.CloseBrace");

        public static readonly ComposableTokenType AttributeNamePrefix = new ComposableTokenType("Angle.AttributeNamePrefix");
        public static readonly ComposableTokenType AttributeNameSeparator = new ComposableTokenType("Angle.AttributeNameSeparator");
        public static readonly ComposableTokenType AttributeName = new ComposableTokenType("Angle.AttributeName");
        public static readonly ComposableTokenType AttributeValueSeparator = new ComposableTokenType("Angle.AttributeValueSeparator");
        public static readonly ComposableTokenType AttributeValue = new ComposableTokenType("Angle.AttributeValue");
        public static readonly ComposableTokenType AttributeOpenValue = new ComposableTokenType("Angle.AttributeOpenValue");
        public static readonly ComposableTokenType AttributeCloseValue = new ComposableTokenType("Angle.AttributeCloseValue");

        public static readonly ComposableTokenType OpenDirective = new ComposableTokenType("Angle.OpenDirective");

        public static readonly ComposableTokenType OpenComment = new ComposableTokenType("Angle.OpenComment");
        public static readonly ComposableTokenType CloseComment = new ComposableTokenType("Angle.CloseComment");

        private AngleTokenType() 
            : base("")
        { }
    }
}
