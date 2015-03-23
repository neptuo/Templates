using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    [DebuggerDisplay("{ContentInfo} ({Type}) {Text}")]
    public class CurlyToken : IToken<CurlyTokenType>
    {
        public CurlyTokenType Type { get; set; }
        public ISourceRangeLineInfo LineInfo { get; set; }
        public ISourceContentInfo ContentInfo { get; set; }
        public string Text { get; set; }
        public IErrorMessage Error { get; set; }
    }
}
