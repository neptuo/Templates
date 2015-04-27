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
        public static readonly ComposableTokenType CloseBrace = new ComposableTokenType("Angle.CloseBrace");

        private AngleTokenType() 
            : base("")
        { }
    }
}
