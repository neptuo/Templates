using Neptuo.ComponentModel;
using Neptuo.Templates.Compilation.Parsers.Tokenizers.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Templates.Compilation.Parsers.Tokenizers
{
    /// <summary>
    /// Token implementation for <see cref="ComposableTokenizer"/>.
    /// </summary>
    public class ComposableToken : IToken<ComposableTokenType>
    {
        public ComposableTokenType Type { get; set; }
        public ISourceRangeLineInfo LineInfo { get; set; }
        public ISourceContentInfo ContentInfo { get; set; }
        public string Text { get; set; }
        public IErrorMessage Error { get; set; }
    }
}
