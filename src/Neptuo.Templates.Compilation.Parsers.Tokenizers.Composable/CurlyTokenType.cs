using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    public class CurlyTokenType : ComposableTokenType
    {
        public static readonly ComposableTokenType OpenBrace = new ComposableTokenType("Curly.OpenBrace");
        public static readonly ComposableTokenType NamePrefix = new ComposableTokenType("Curly.NamePrefix");
        public static readonly ComposableTokenType NameSeparator = new ComposableTokenType("Curly.NameSeparator");
        public static readonly ComposableTokenType Name = new ComposableTokenType("Curly.Name");

        public static readonly ComposableTokenType DefaultAttributeValue = new ComposableTokenType("Curly.DefaultAttributeName");
        public static readonly ComposableTokenType AttributeSeparator = new ComposableTokenType("Curly.AttributeSeparator");

        public static readonly ComposableTokenType AttributeName = new ComposableTokenType("Curly.AttributeName");
        public static readonly ComposableTokenType AttributeValueSeparator = new ComposableTokenType("Curly.AttributeValueSeparator");

        public static readonly ComposableTokenType CloseBrace = new ComposableTokenType("Curly.CloseBrace");

        private CurlyTokenType() 
            : base("")
        { }
    }
}
